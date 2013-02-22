using System;

namespace AOPify.Aspects.ConsoleTests
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            MothodsTest();
            Console.ReadLine();
        }

        private static void MothodsTest()
        {
            CustomerRepository customerRepository = new CustomerRepository();
            Customer customer = customerRepository.GetCustomerById(10);
          
            if (customer != null)
            {
                Console.WriteLine(customer.Id);
            }
        }
    }
}
