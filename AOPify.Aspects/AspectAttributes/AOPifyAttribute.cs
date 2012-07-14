using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using AOPify.Aspects.AspectProperties;
using AOPify.Aspects.Enum;

namespace AOPify.Aspects.AspectAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AOPifyAttribute : ContextAttribute
    {
        private readonly ProcessMode _processMode;
        private readonly Type _processorType;

        //TODO: make params
        public AOPifyAttribute(ProcessMode all, Type processorType)
            : base("AOPify")
        {
            _processMode = all;
            _processorType = processorType;
        }
        public AOPifyAttribute()
            : base("AOPify")
        {

        }

        public override void Freeze(Context newContext)
        {

        }

        public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
        {
            AOPifyProperty property;
            if(_processorType!=null)
          property = new AOPifyProperty(_processMode, _processorType);
        else
            {
                property=new AOPifyProperty();
            }
            ctorMsg.ContextProperties.Add(property);
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
