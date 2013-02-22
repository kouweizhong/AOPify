using System;
using AOPify.Aspects.Contexts;
using AOPify.Aspects.Interface;

namespace AOPify.Aspects.ConsoleTests
{
    public class ConsolePostProcessor : IPostAspectProcessor
    {
        public void Process(PostProcessContext context)
        {
            Console.WriteLine(context.ReturnContext.ReturnValue);
        }
    }
}
