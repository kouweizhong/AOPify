using AOPify.Aspects.Contexts;
using AOPify.Aspects.Sinks;

namespace AOPify.Aspects.Interface
{
    public interface IPreAspectProcessor
    {
        void Process(PreProcessContext context);
    }
}
