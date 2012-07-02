using System;
using System.Reflection;

namespace AOPify.Aspects.ConsoleTests
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            CustomerRepository customerRepository = new CustomerRepository();
            Console.WriteLine(customerRepository.GetCustomerByID(10));
            //customerRepository.ThrowException();
            //Use hybrid
            AOPify.Let
               .Catch(exception => Console.WriteLine(exception.Message))
               .Run(customerRepository.ThrowException);


            Console.ReadLine();
        }
    }
}
