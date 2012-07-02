using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AOPify.Common
{
    internal static class ExceptionUtils
    {
        public static void ExecuteWithFilter(Action body, Func<Exception, bool> filter, Action<Exception> handler)
        {
            s_filter(body, filter, handler);
        }
        public static void ExecuteWithFilter(Action body, Action<Exception> filter)
        {
            ExecuteWithFilter(body, e => { filter(e); return false; }, null);
        }
        public static void ExecuteWithFailfast(Action body)
        {
            ExecuteWithFilter(body, e =>
                                        {
                                            Debugger.Log(10, "ExceptionFilter", "Saw unexpected exception: " + e.ToString());
                                            if (Environment.Version.Major >= 4)
                                            {
                                                typeof(Environment).InvokeMember("FailFast",
                                                                                 BindingFlags.Static | BindingFlags.InvokeMethod,
                                                                                 null, null, new object[] { "Unexpected Exception", e });
                                            }
                                            else
                                            {
                                                Environment.FailFast("Exception: " + e.GetType().FullName);
                                            }

                                            return false;
                                        }, null);
        }

        public static void TryCatchMultiple<TExceptionBase>(Action body, Type[] typesToCatch, Action<TExceptionBase> handler)
            where TExceptionBase : Exception
        {
#if DEBUG
            foreach(var tc in typesToCatch)
                Debug.Assert(typeof(TExceptionBase).IsAssignableFrom(tc), String.Format("Error: {0} is not a sub-class of {1}",
                    tc.FullName, typeof(TExceptionBase).FullName));
#endif

            ExecuteWithFilter(body, e => typesToCatch.Any(catchType => catchType.IsInstanceOfType(e)), e => handler((TExceptionBase)e));
        }

        public static void TryCatchMultiple(Action body, Type[] typesToCatch, Action<Exception> handler)
        {
            TryCatchMultiple<Exception>(body, typesToCatch, handler);
        }
     
        private static bool k_debug;

        private static Action<Action, Func<Exception, bool>, Action<Exception>> GenerateFilter()
        {
            // Create a dynamic assembly with reflection emit
            var name = new AssemblyName("DynamicFilter");
            AssemblyBuilder assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(name, k_debug ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run);
            ModuleBuilder module;
            if (k_debug)
                module = assembly.DefineDynamicModule("DynamicFilter", "DynamicFilter.dll");
            else
                module = assembly.DefineDynamicModule("DynamicFilter");

            assembly.SetCustomAttribute(new CustomAttributeBuilder(
                                            typeof(RuntimeCompatibilityAttribute).GetConstructor(new Type[] { }),
                                            new object[] { },
                                            new PropertyInfo[] { typeof(RuntimeCompatibilityAttribute).GetProperty("WrapNonExceptionThrows") },
                                            new object[] { true }));

            assembly.SetCustomAttribute(new CustomAttributeBuilder(
                                            typeof(DebuggableAttribute).GetConstructor(new[] { typeof(DebuggableAttribute.DebuggingModes) }),
                                            new object[] { DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints }));

            TypeBuilder type = module.DefineType("Filter", TypeAttributes.Class | TypeAttributes.Public);
            var argTypes = new Type[] { typeof(Action), typeof(Func<Exception, bool>), typeof(Action<Exception>) };
            MethodBuilder meth = type.DefineMethod("InvokeWithFilter", MethodAttributes.Public | MethodAttributes.Static, typeof(void), argTypes);

            var il = meth.GetILGenerator();
            il.DeclareLocal(typeof(Exception));

            // Invoke the body delegate inside the try
            il.BeginExceptionBlock();
            il.Emit(OpCodes.Ldarg_0);
            il.EmitCall(OpCodes.Callvirt, typeof(Action).GetMethod("Invoke"), null);

            // Invoke the filter delegate inside the filter block
            il.BeginExceptFilterBlock();
            il.Emit(OpCodes.Castclass, typeof(Exception));
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldloc_0);
            il.EmitCall(OpCodes.Callvirt, typeof(Func<Exception, bool>).GetMethod("Invoke"), null);

            // Invoke the handler delegate inside the catch block
            il.BeginCatchBlock(null);
            il.Emit(OpCodes.Castclass, typeof(Exception));
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Ldloc_0);
            il.EmitCall(OpCodes.Callvirt, typeof(Action<Exception>).GetMethod("Invoke"), null);

            il.EndExceptionBlock();
            il.Emit(OpCodes.Ret);

            var bakedType = type.CreateType();
            if (k_debug)
                assembly.Save("DynamicFilter.dll");

            // Construct a delegate to the filter function and return it
            var bakedMeth = bakedType.GetMethod("InvokeWithFilter");
            var del = Delegate.CreateDelegate(typeof(Action<Action, Func<Exception, bool>, Action<Exception>>), bakedMeth);
            return (Action<Action, Func<Exception, bool>, Action<Exception>>)del;
        }

        // Will get generated (with automatic locking) on first use of this class
        private static Action<Action, Func<Exception, bool>, Action<Exception>> s_filter = GenerateFilter();
    }
}