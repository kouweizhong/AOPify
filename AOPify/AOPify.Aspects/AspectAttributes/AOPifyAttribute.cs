using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using AOPify.Aspects.AspectProperties;

namespace AOPify.Aspects.AspectAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AOPifyAttribute : ContextAttribute
    {
        public AOPifyAttribute()
            : base("AOPify")
        {
        }

        public override void Freeze(Context newContext)
        {
        }

        public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
        {
            ctorMsg.ContextProperties.Add(new AOPifyProperty());
        }

        public override bool IsContextOK(Context context, IConstructionCallMessage ctorMsg)
        {
            AOPifyProperty aoPifyProperty = context.GetProperty("AOPify") as AOPifyProperty;

            return aoPifyProperty != null;
        }

        public override bool IsNewContextOK(Context newContext)
        {
            AOPifyProperty aoPifyProperty = newContext.GetProperty("AOPify") as AOPifyProperty;

            return aoPifyProperty != null;
        }
    }
}
