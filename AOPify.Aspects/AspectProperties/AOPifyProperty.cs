using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using AOPify.Aspects.Enum;
using AOPify.Aspects.Sinks;

namespace AOPify.Aspects.AspectProperties
{
    internal class AOPifyProperty : IContextProperty, IContributeObjectSink //IContributeServerContextSink
    {
        private readonly ProcessMode _processMode;
        private readonly Type _processorType;

        public AOPifyProperty(ProcessMode processMode, Type processorType)
        {
            _processMode = processMode;
            _processorType = processorType;
        }

        public AOPifyProperty()
        {
        }

        #region IContextProperty Members

        public string Name
        {
            get
            {
                return "AOPify";
            }
        }

        public bool IsNewContextOK(Context newContext)
        {
            AOPifyProperty property = newContext.GetProperty("AOPify") as AOPifyProperty;
            return property != null;
        }

        public void Freeze(Context newContext)
        {
            //todo: 
        }

        #endregion

        #region IContributeObjectSink Members

        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            AOPifySink sink;
            if (_processorType != null)
            {
               sink= new AOPifySink(nextSink, _processMode, _processorType);
            }
            else
            {
                sink=new AOPifySink(nextSink);
            }
            return sink;
        }

        #endregion
    }
}
