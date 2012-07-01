using System.Runtime.Remoting.Messaging;

namespace AOPify.Aspects.Contexts
{
    public class MethodCallContext : MethodCallMessageWrapper
    {
        public MethodCallContext(ref IMethodCallMessage methodCallMessage) : base(methodCallMessage)
        {
        }
    }
}