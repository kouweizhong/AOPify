using System;
using AOPify.Aspects.Contexts;
using AOPify.Aspects.IProcessor;

namespace AOPify.Aspects.ConsoleTests
{
    public class ConsolePreAspectProcessor : IPreAspectProcessor
    {
        public void Process(ref MethodCallContext callContext)
        {
            // Log(String.Format("PreProcessing:{0}", callContext.MethodName));
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
