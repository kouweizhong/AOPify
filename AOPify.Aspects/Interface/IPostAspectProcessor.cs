using AOPify.Aspects.Contexts;
using AOPify.Aspects.Sinks;

namespace AOPify.Aspects.Interface
{
    public interface IPostAspectProcessor
    {
        void Process(PostProcessContext preProcessContext);
    }
}
