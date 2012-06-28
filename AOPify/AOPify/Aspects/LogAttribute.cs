using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Activation;
using AOPify.Enum;

namespace AOPify.Aspects
{
    public class LogAttribute : ContextAttribute
    {
        private readonly Mode[] _logMode;
        private readonly IAOPLogger _consoleLogger;
        public LogAttribute(Type loggerType, params  Mode[] logModes)
            : base("Log")
        {
            _logMode = logModes;
            _consoleLogger = (IAOPLogger)Activator.CreateInstance(loggerType);
        }

        public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
        {
            ctorMsg.ContextProperties.Add(new LogProperty(_consoleLogger, _logMode));
        }
    }
}
