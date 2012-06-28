using System;
using AOPify.Aspects;
using AOPify.Enum;

namespace AOPify.Test
{
    [Log(typeof(ConsoleLogger), Mode.OnBefore, Mode.OnAfter, Mode.HowLong, Mode.OnError, Mode.WithReturnType)]
    public class CustomerRepository : AspectObject
    {
        public void DoSomething(string script)
        {
            Console.WriteLine("Script executed: {0}", script);
        }

        public Customer GetCustomer(int customerID,string name)
        {
            return new Customer(customerID);
        }
    }
}
