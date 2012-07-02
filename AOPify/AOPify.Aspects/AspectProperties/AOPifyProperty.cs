using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using AOPify.Aspects.Sinks;

namespace AOPify.Aspects.AspectProperties
{
    public class AOPifyProperty : IContextProperty, IContributeObjectSink //IContributeServerContextSink
    {
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
            return new AOPifySink(nextSink);
        }

        #endregion
    }
}
