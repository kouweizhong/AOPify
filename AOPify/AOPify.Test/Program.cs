using System;

namespace AOPify.Test
{
    class Program
    {
        static void Main()
        {
            AOPify.Let
                .Before(() => Console.WriteLine("Before"))
                .After(() => Console.WriteLine("After"))
                .Run(() => Console.WriteLine("Actual"));

            //-------------------------
            AOPify.Let.Catch(ex=> Console.WriteLine(ex.Message)).Run(()=>
            {
                Console.WriteLine("Run");
                throw  new Exception("Exception");
            });
            Console.Read();
        }
    }
}
