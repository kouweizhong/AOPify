using AOPify.Aspects.Contexts;

namespace AOPify.Aspects.Sinks
{
    internal class ContextFactory
    {
        public static PreProcessContext CreatePreProcessContext(MethodCallContext callContext)
        {
            PreProcessContext preProcessContext = new PreProcessContext();
            preProcessContext.CallContext = callContext;
            return preProcessContext;
        }

        public static PostProcessContext CreatePostProcessContext(MethodCallContext callContext,ref MethodReturnContext returnContext)
        {
            PostProcessContext context = new PostProcessContext(ref returnContext);
            context.CallContext = callContext;
            return context;
        }
    }
}