using AOPify.Aspects.Contexts;

namespace AOPify.Aspects.IProcessor
{
	public interface IPostAspectProcessor : ILoggable
	{
	   void Process(MethodCallContext callContext,ref MethodReturnContext returnContext);
	}
}
