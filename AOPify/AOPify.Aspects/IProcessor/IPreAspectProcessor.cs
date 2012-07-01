using AOPify.Aspects.Contexts;

namespace AOPify.Aspects.IProcessor
{
    public interface IPreAspectProcessor : ILoggable
    {
        void Process(ref MethodCallContext callContext);
    }
}
