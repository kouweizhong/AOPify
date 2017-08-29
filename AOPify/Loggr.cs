using System;
using AOPify.Logging;
using System.Reflection;

namespace AOPify
{
    public class Loggr
    {
        internal Type Target;
        internal ILogger Logger;
        internal MethodBase CurrentMethod;

        public static Loggr Instance => new Loggr();

        public Loggr For<T>(T target)
        {
            Target = target.GetType();
            return this;
        }
        public Loggr For(Type type)
        {
            Target = type;
            return this;
        }

        public Loggr Using<T>() where T : ILogger
        {
            Logger = Activator.CreateInstance<T>();
            return this;
        }
        public Loggr Using<T>(T logger) where T : ILogger
        {
            Logger = logger;
            return this;
        }
    }
}