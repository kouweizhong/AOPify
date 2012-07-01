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
            ctorMsg.ContextProperties.Add(new InterceptProperty());
        }

        public override bool IsContextOK(Context context, IConstructionCallMessage ctorMsg)
        {
            InterceptProperty interceptProperty = context.GetProperty("AOPify") as InterceptProperty;

            return interceptProperty != null;
        }

        public override bool IsNewContextOK(Context newContext)
        {
            InterceptProperty interceptProperty = newContext.GetProperty("AOPify") as InterceptProperty;

            return interceptProperty != null;
        }
    }
}
