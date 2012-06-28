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
        internal IAOPLogger Logger;
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

        public Log Use<T>() where T : IAOPLogger
        {
            Logger = Activator.CreateInstance<T>();
            return this;
        }
        public Log Use<T>(T logger) where T : IAOPLogger
        {
            Logger = logger;
            return this;
        }
    }
}