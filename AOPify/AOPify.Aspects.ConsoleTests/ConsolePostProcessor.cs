using System;
using AOPify.Aspects.Contexts;
using AOPify.Aspects.IProcessor;

namespace AOPify.Aspects.ConsoleTests
{
    public class ConsolePostAspectProcessor : IPostAspectProcessor
    {
        public void Process(MethodCallContext callContext, ref MethodReturnContext returnContext)
        {
            //Log(String.Format("Return:{0}", returnContext.ReturnValue));
            //Log(String.Format("PostProcessing:{0}", callContext.MethodName));
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
