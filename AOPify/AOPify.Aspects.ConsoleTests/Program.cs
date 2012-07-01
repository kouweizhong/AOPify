using System;

namespace AOPify.Aspects.ConsoleTests
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            CustomerRepository customerRepository = new CustomerRepository();
            Console.WriteLine(customerRepository.GetCustomerByID(10));
            customerRepository.ThrowException();

            Console.ReadLine();
        }
    }
}
