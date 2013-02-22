using System;
using AOPify.Aspects.Contexts;
using AOPify.Aspects.Interface;

namespace AOPify.Aspects.ConsoleTests
{
    public class ConsolePreProcessor : IPreAspectProcessor
    {
        public void Process(PreProcessContext context)
        {
            Console.WriteLine(context.CallContext.MethodName);
        }
    }
}
