using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using AOPify.Aspects.Sinks;

namespace AOPify.Aspects.AspectProperties
{
    //IContextProperty, IContributeServerContextSink
    public class InterceptProperty : IContextProperty, IContributeObjectSink
    {
        #region IContextProperty Members

        public string Name
        {
            get
            {
                return "Intercept";
            }
        }

        public bool IsNewContextOK(Context newContext)
        {
            InterceptProperty property = newContext.GetProperty("Intercept") as InterceptProperty;
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
