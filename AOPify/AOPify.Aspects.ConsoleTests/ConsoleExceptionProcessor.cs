using System;
using AOPify.Aspects.Processors;

namespace AOPify.Aspects.ConsoleTests
{
    public class ConsoleExceptionAspectProcessor : ExceptionHandlingAspectProcessor
    {
        public override void HandleException(Exception exception)
        {
            Console.WriteLine(exception.ToString());
        }
    }
}
