using System;

namespace AOPify.Aspects.ConsoleTests
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            CustomerRepository customerRepository = new CustomerRepository();
            Console.WriteLine(customerRepository.GetCustomerById(10));
            Customer customer = customerRepository.ThrowException();

            Console.ReadLine();
        }

    }
}
