using System;
using System.Runtime.Remoting.Messaging;
using AOPify.Aspects.Attributes;
using AOPify.Aspects.Common;
using AOPify.Aspects.Contexts;
using AOPify.Aspects.Enum;

namespace AOPify.Aspects.Sinks
{
    internal class AOPifySink : IMessageSink
    {
        private ExecutionTimer _timer;
        private readonly IMessageSink _nextSink;

        public AOPifySink(IMessageSink nextSink)
        {
            _nextSink = nextSink;
        }

        public IMessageSink NextSink
        {
            get
            {
                return _nextSink;
            }
        }

        #region IMessageSink Members

        public IMessage SyncProcessMessage(IMessage message)
        {
            IMethodCallMessage methodCallMessage = (message as IMethodCallMessage);
            MethodCallContext callContext = new MethodCallContext(ref methodCallMessage);
            PreProcess(ref callContext);

            //Todo: try catch issue
            _timer = new ExecutionTimer();
            _timer.Start(callContext.MethodName);

            IMessage returnMessage = _nextSink.SyncProcessMessage(message);
            IMethodReturnMessage methodReturnMessage = (returnMessage as IMethodReturnMessage);
            PostProcess(message as IMethodCallMessage, methodReturnMessage);

            return methodReturnMessage;
        }



        public IMessageCtrl AsyncProcessMessage(IMessage message, IMessageSink replySink)
        {
            IMessageCtrl rtnMsgCtrl = _nextSink.AsyncProcessMessage(message, replySink);
            return rtnMsgCtrl;
        }

        #endregion

        private void PreProcess(ref MethodCallContext callContext)
        {
            PreProcessAttribute[] attributes = ReflectionHelper.GetAttributes<PreProcessAttribute>(callContext, inherit: true);

            foreach (PreProcessAttribute attribute in attributes)
            {
                PreProcessMode[] preProcessModes = attribute.GetProcessModes();

                foreach (PreProcessMode processMode in preProcessModes)
                {
                    ProcessBeforeModeOperation(processMode, attribute, callContext);
                }

                attribute.AspectProcessor.Process(ref callContext);
            }
        }


        private void PostProcess(IMethodCallMessage callMsg, IMethodReturnMessage returnMessage)
        {
            PostProcessAttribute[] attributes = ReflectionHelper.GetAttributes<PostProcessAttribute>(callMsg, inherit: true);

            foreach (PostProcessAttribute attribute in attributes)
            {
                MethodCallContext callContext = new MethodCallContext(ref callMsg);
                MethodReturnContext returnContext = new MethodReturnContext(returnMessage);

                foreach (PostProcessMode processMode in attribute.GetProcessModes())
                {
                    ProcessPostModeOperation(processMode, attribute, callContext, returnContext);
                }
                attribute.AspectProcessor.Process(callContext, ref returnContext);
            }
        }

        private void ProcessBeforeModeOperation(PreProcessMode processMode, PreProcessAttribute attribute, MethodCallContext callContext)
        {
            switch (processMode)
            {
                case PreProcessMode.OnBefore:
                    attribute.AspectProcessor.Log("Method call started: {0}".FormatWith(callContext.MethodName));
                    break;

                case PreProcessMode.WithInputParameters:
                    string parameters = AspectHelper.GetFormattedInputParameters(callContext);
                    attribute.AspectProcessor.Log("Method call started: {0}{1}".FormatWith(callContext.MethodName, parameters));
                    break;
            }
        }

        private void ProcessPostModeOperation(PostProcessMode processMode, PostProcessAttribute attribute, MethodCallContext callContext, MethodReturnContext returnContext)
        {
            switch (processMode)
            {
                case PostProcessMode.OnAfter:
                    attribute.AspectProcessor.Log("Method call ended: {0}".FormatWith(callContext.MethodName));
                    break;
                case PostProcessMode.HowLong:
                    _timer.Finish();
                    attribute.AspectProcessor.Log("Total time for {0}:{1}ms".FormatWith(_timer.Operation, _timer.Duration.Milliseconds));
                    break;
                case PostProcessMode.OnError:
                    if (returnContext.Exception != null)
                    {
                        attribute.AspectProcessor.Log(
                            "An error occured while method call: {0}".FormatWith(returnContext.Exception.Message));
                    }
                    break;
                case PostProcessMode.WithReturnType:
                    //Todo:ZYAS --> Reflection for return value parameters
                    //PropertyInfo[] propertyInfos = returnMessage.ReturnValue.GetType().GetProperties(BindingFlags.Public);
                    attribute.AspectProcessor.Log("Method {0}, return Type: {1}".FormatWith(returnContext.MethodName, returnContext.ReturnValue));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("processMode");
            }
        }
    }
}