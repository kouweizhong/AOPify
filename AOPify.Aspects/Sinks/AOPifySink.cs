using System.Linq;
using System.Runtime.Remoting.Messaging;
using AOPify.Aspects.Attributes;
using AOPify.Aspects.Common;
using AOPify.Aspects.Contexts;

namespace AOPify.Aspects.Sinks
{
    internal class AOPifySink : IMessageSink
    {
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

            if (attributes.Any())
            {
                PreProcessAttribute attribute = attributes.FirstOrDefault();

                if (attribute != null)
                {
                    PreProcessContext context = ContextFactory.CreatePreProcessContext(callContext);
                    attribute.AspectProcessor.Process(context);
                }
                ;
            }
        }


        private void PostProcess(IMethodCallMessage callMsg, IMethodReturnMessage returnMessage)
        {
            PostProcessAttribute[] attributes = ReflectionHelper.GetAttributes<PostProcessAttribute>(callMsg, inherit: true);

            if (attributes.Any())
            {
                PostProcessAttribute attribute = attributes.FirstOrDefault();
                if (attribute != null)
                {
                    MethodCallContext callContext = new MethodCallContext(ref callMsg);
                    MethodReturnContext returnContext = new MethodReturnContext(returnMessage);

                    PostProcessContext context = ContextFactory.CreatePostProcessContext(callContext,ref returnContext);
                    attribute.AspectProcessor.Process(context);
                }
            }
        }
    }
}