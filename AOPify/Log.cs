using System;
using System.Reflection;

namespace AOPify
{
    public class Log
    {
        public static Log It
        {
            get
            {
                return new Log();
            }
        }
        internal Type Target;
        internal ILogger Logger;
        internal MethodBase CurrentMethod;

        public Log For<T>(T target)
        {
            Target = target.GetType();
            return this;
        }
        public Log For(Type type)
        {
            Target = type;
            return this;
        }

        public Log Use<T>() where T : ILogger
        {
            Logger = Activator.CreateInstance<T>();
            return this;
        }
        public Log Use<T>(T logger) where T : ILogger
        {
            Logger = logger;
            return this;
        }
    }
}