using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using AOPify.Common;
using AOPify.Enum;

namespace AOPify.Aspects
{
    public class LogAspect : IMessageSink
    {
        public IMessageSink NextSink
        {
            get { return _nextSink; }
        }

        private readonly IMessageSink _nextSink;
        private readonly IAOPLogger _aopLogger;
        private readonly Mode[] _logModes;

        public LogAspect(IMessageSink nextSink, IAOPLogger aopLogger, Mode[] logModes)
        {
            _nextSink = nextSink;
            _aopLogger = aopLogger;
            _logModes = logModes;
        }

        public IMessage SyncProcessMessage(IMessage msg)
        {
            DateTime executionTime = DateTime.Now;

            if (IsModeCalled(Mode.OnBefore))
            {
                executionTime = OnMethodRun(msg); //join to method
            }

            IMessage returnMethod = _nextSink.SyncProcessMessage(msg); //method execute

            if (IsModeCalled(Mode.OnAfter))
            {
                AfterMethodRun(msg, returnMethod, executionTime); //after method execute
            }

            return returnMethod;
        }

        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return default(IMessageCtrl);
        }

        private void AfterMethodRun(IMessage msg, IMessage returnMethod, DateTime executionTime)
        {
            //Message should be a IMethodMessage or IMethodReturnMessage
            bool isMethodMessage = returnMethod is IMethodReturnMessage || msg is IMethodMessage;

            if (!isMethodMessage)
            {
                return;
            }

            IMethodReturnMessage returnMessage = (IMethodReturnMessage)returnMethod;
            //todo: exception handling
            bool hasExceptionOccured = returnMessage.Exception != null;

            if (hasExceptionOccured)
            {
                if (IsModeCalled(Mode.OnError))
                {
                    _aopLogger.Info("An error occured while method call: {0}", returnMessage.Exception.Message);
                    //todo resturn statement
                }
                return;
            }

            if (IsModeCalled(Mode.WithReturnType))
            {
                //Todo: Reflection for return value parameters
                //PropertyInfo[] propertyInfos = returnMessage.ReturnValue.GetType().GetProperties(BindingFlags.Public);

                _aopLogger.Info("Method return Type: {0}", returnMessage.ReturnValue);
            }

            if (IsModeCalled(Mode.HowLong))
            {
                _aopLogger.Info("END - Method execution time : {0} Milliseconds",
                                DateTime.Now.Subtract(executionTime).Milliseconds);
            }

        }

        private bool IsModeCalled(Mode mode)
        {
            return _logModes.Contains(mode);
        }

        private DateTime OnMethodRun(IMessage msg)
        {
            //Message should be a IMethodMessage.
            bool isMethodMessage = msg is IMethodMessage;

            if (!isMethodMessage)
            {
                return DateTime.MinValue;
            }

            DateTime executionTime = DateTime.Now;
            IMethodMessage methodMessageCall = msg as IMethodMessage;
            string parameters = AspectHelper.GetFormattedInputParameters(methodMessageCall);

            if (IsModeCalled(Mode.WithInputParameters))
            {
                _aopLogger.Info("START - Method call started: {0}{1} - Execution Time : {2}",methodMessageCall.MethodName, parameters, executionTime);
            }
            else
            {
                _aopLogger.Info("START - Method call started: {0} - Execution Time : {1}", methodMessageCall.MethodName, executionTime);
            }


            return executionTime;
        }


    }
}
