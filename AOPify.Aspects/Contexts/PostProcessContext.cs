namespace AOPify.Aspects.Contexts
{
    public class PostProcessContext :ProcessContext
    {
        public PostProcessContext(ref MethodReturnContext returnContext)
        {
            ReturnContext = returnContext;
        }

        public MethodReturnContext ReturnContext { get; set; }
    }
}