using System;
using System.Reflection;

namespace AOPify.ConsoleSample
{
    class Program
    {
        static void Main()
        {
            //Before and After
            ConsoleLogger consoleLogger = new ConsoleLogger();
            AOP.Instance
                .Before(() => Console.WriteLine("Before"))
                .After(() => Console.WriteLine("After"))
                .Run(() => Console.WriteLine("Actual"));

            Console.WriteLine("-----------------------------------------------------------------------------------------------");

            //Catch
            AOP.Instance.Catch(ex => Console.WriteLine(ex.Message)).Run(() =>
            {
                Console.WriteLine("Run with Error");
                throw new Exception("Exception");
            });

            Console.WriteLine("-----------------------------------------------------------------------------------------------");

            //Register logger with logger instance
            AOP.Instance
                .RegisterLogger(Loggr.Instance.For(typeof(Program)).Using(consoleLogger))
                .Log("Before Log {0}".FormatWith(MethodBase.GetCurrentMethod().Name),
                    "After Log {0}".FormatWith(MethodBase.GetCurrentMethod().Name))
                .Run(() => Console.WriteLine("Run executed"));

            Console.WriteLine("-----------------------------------------------------------------------------------------------");

            //Register logger with logger Generic (system will create new instance)
            AOP.Instance
                .RegisterLogger(Loggr.Instance.For(typeof(Program)).Using<ConsoleLogger>())
                .Log(MethodBase.GetCurrentMethod())
                .Run(() => Console.WriteLine("Run executed"));

            Console.WriteLine("-----------------------------------------------------------------------------------------------");

            //HowLong
            AOP.Instance
               .RegisterLogger(Loggr.Instance.For(typeof(Program)).Using<ConsoleLogger>())
               .Log(MethodBase.GetCurrentMethod())
               .HowLong()
               .Run(() => Console.WriteLine("Run executed with How Long"));

            Console.WriteLine("-----------------------------------------------------------------------------------------------");

            //Other Method test
            MyMethod("Test", 5, 8);

            Console.WriteLine("-----------------------------------------------------------------------------------------------");

            //Delay
            AOP.Instance
               .RegisterLogger(Loggr.Instance.For(typeof(Program)).Using<ConsoleLogger>())
               .Log(MethodBase.GetCurrentMethod())
               .Delay(10000)
               .Run(() => Console.WriteLine("Delay : Run executed"));

            Console.WriteLine("-----------------------------------------------------------------------------------------------");

            //Until
            AOP.Instance
               .RegisterLogger(Loggr.Instance.For(typeof(Program)).Using<ConsoleLogger>())
               .Log(MethodBase.GetCurrentMethod())
               .Until(() => new Random().Next(0, 100) % 5 == 0)
               .Run(() => Console.WriteLine("Until : Run executed"));


            Console.Read();
        }

        private static void MyMethod(string testStr, int count, int orderNo)
        {
            AOP.Instance
             .RegisterLogger(Loggr.Instance.For(typeof(Program)).Using<ConsoleLogger>())
             .Log(MethodBase.GetCurrentMethod())
             .Run(() => Console.WriteLine("Run Executed"));
        }
    }
}
