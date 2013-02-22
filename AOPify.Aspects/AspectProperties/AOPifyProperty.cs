using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using AOPify.Aspects.Sinks;

namespace AOPify.Aspects.AspectProperties
{
    internal class AOPifyProperty : IContextProperty, IContributeObjectSink //IContributeServerContextSink
    {
        private readonly Type _processorType;

        public AOPifyProperty(Type processorType)
        {
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

        public Type ProcessorType
        {
            get { return _processorType; }
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
            AOPifySink sink = new AOPifySink(nextSink);
            return sink;
        }

        #endregion
    }
}
