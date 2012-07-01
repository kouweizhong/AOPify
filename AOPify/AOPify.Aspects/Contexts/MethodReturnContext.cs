using System.Runtime.Remoting.Messaging;

namespace AOPify.Aspects.Contexts
{
    public class MethodReturnContext : MethodReturnMessageWrapper
    {
        public MethodReturnContext(IMethodReturnMessage methodReturnMessage)
            : base(methodReturnMessage)
        {
        }
    }
}