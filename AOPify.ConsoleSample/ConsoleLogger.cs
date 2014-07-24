using System;
using AOPify.Logging;

namespace AOPify.ConsoleSample
{
    public class ConsoleLogger : ILogger
    {

        public void Debug(string message)
        {
        }

        public void DebugException(string message, Exception exception)
        {
        }

        public void Error(string message)
        {
        }

        public void Error(string message, Exception exception)
        {
            Console.WriteLine("Ex Msg :{0}, Custom message :{1}".FormatWith(exception.Message, message));
        }

        public void Info(string message)
        {

        }

        public void Info(string message, Exception exception)
        {

        }

        public void Warn(string message)
        {
        }

        public void Warn(string message, Exception exception)
        {
        }

        public void Fatal(string message)
        {
        }

        public void Fatal(string message, Exception exception)
        {
        }
    }
}