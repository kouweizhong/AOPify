using System;

namespace AOPify.Aspects.ConsoleTests
{
    /// <summary>
    /// Create a class for your log operations
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void Log(string format, params object[] args)
        {
            Console.WriteLine(format.FormatWith(args));
        }

        public void Error(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }

        public void Error(Exception exception, string message)
        {
            Console.WriteLine("Ex Msg :{0}, Custom message :{1}".FormatWith(exception.Message, message));
        }

        public void Info(string format, params object[] args)
        {
            Log(format, args);
        }
    }
}