using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using AOPify.Enum;

namespace AOPify.Aspects
{
    public class LogProperty : IContextProperty, IContributeObjectSink
    {
        private readonly IAOPLogger _aopLogger;
        private readonly Mode[] _logMode;

        public LogProperty(IAOPLogger aopLogger, Mode[] logMode)
        {
            _aopLogger = aopLogger;
            _logMode = logMode;
        }

        //IContributeObjectSink Members
        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            return new LogAspect(nextSink,_aopLogger,_logMode);
        }

        //IContextProperty Members
        public void Freeze(Context newContext)
        {
            
        }
        public bool IsNewContextOK(Context newCtx)
        {
            return true;
        }

        public string Name
        {
            get { return "LogProperty"; }
        }
    }
}
