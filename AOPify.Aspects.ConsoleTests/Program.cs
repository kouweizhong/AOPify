using System;
using System.Reflection.Emit;
using AOPify.Common;

namespace AOPify.Aspects.ConsoleTests
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            CustomerRepository customerRepository = new CustomerRepository();
            Console.WriteLine(customerRepository.GetCustomerByID(10));
            Customer customer = customerRepository.ThrowException();

            Console.ReadLine();
        }
        public static void Throw(bool fatal)
        {
            Console.WriteLine("Throw Executed");
            //if (fatal) throw new InvalidOperationException("Boom!");
        }
        public static void Log(Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

    }
}
