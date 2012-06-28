using System;
using System.Reflection;

namespace AOPify.Test
{
    class Program
    {
        static void Main()
        {
            //Before and After
            ConsoleLogger consoleLogger = new ConsoleLogger();
            AOPify.Let
                .Before(() => Console.WriteLine("Before"))
                .After(() => Console.WriteLine("After"))
                .Run(() => Console.WriteLine("Actual"));

            //Catch
            AOPify.Let.Catch(ex => Console.WriteLine(ex.Message)).Run(() =>
            {
                Console.WriteLine("Run with Error");
                throw new Exception("Exception");
            });


            //Register logger with logger instance
            AOPify.Let
                .RegisterLogger(Log.It.For(typeof(Program)).Use(consoleLogger))
                .Log("Before Log {0}".FormatWith(MethodBase.GetCurrentMethod().Name), "After Log {0}".FormatWith(MethodBase.GetCurrentMethod().Name))
                .Run(() => Console.WriteLine("Run executed"));

            //Register logger with logger Generic (system will create new instance)
            AOPify.Let
                .RegisterLogger(Log.It.For(typeof(Program)).Use<ConsoleLogger>())
                .Log(MethodBase.GetCurrentMethod())
                .Run(() => Console.WriteLine("Run executed"));

            //HowLong
            AOPify.Let
               .RegisterLogger(Log.It.For(typeof(Program)).Use<ConsoleLogger>())
               .Log(MethodBase.GetCurrentMethod())
               .HowLong()
               .Run(() => Console.WriteLine("Run executed with How Long"));

            //Other Method test
            MyMethod("Test", 5, 8);


            //[Log(typeof(ConsoleLogger), Mode.OnBefore, Mode.OnAfter, Mode.HowLong, Mode.OnError, Mode.WithReturnType)]
            //Attribute Test (look at to Cuatomer repository)
            CustomerRepository customerRepository = new CustomerRepository();
            Customer customer = customerRepository.GetCustomer(2, "Hugo");
            Console.WriteLine("CustomerID : {0}".FormatWith(customer.CustomerID));
            Console.Read();
        }

        private static void MyMethod(string testStr, int count, int orderNo)
        {
            AOPify.Let
             .RegisterLogger(Log.It.For(typeof(Program)).Use<ConsoleLogger>())
             .Log(MethodBase.GetCurrentMethod())
             .Run(() => Console.WriteLine("Run Executed"));
        }
    }
}
