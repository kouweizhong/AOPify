using System;
using System.Reflection;

namespace AOPify.Test
{
    /// <summary>
    /// Create a class for your log operations
    /// </summary>
    class ConsoleLogger : IAOPLogger
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
    class Program
    {
        static void Main()
        {
            AOPify.Let
                .Before(() => Console.WriteLine("Before"))
                .After(() => Console.WriteLine("After"))
                .Run(() => Console.WriteLine("Actual"));

            //-------------------------
            AOPify.Let.Catch(ex => Console.WriteLine(ex.Message)).Run(() =>
            {
                Console.WriteLine("Run");
                throw new Exception("Exception");
            });


            //------------------------------------------

            AOPify.Let
                .RegisterLogger(new ConsoleLogger())
                .Log("Before Log {0}".FormatWith(MethodBase.GetCurrentMethod().Name), "After Log {0}".FormatWith(MethodBase.GetCurrentMethod().Name))
                .Run(() => Console.WriteLine("Run executed"));


            Console.Read();
        }
    }
}
