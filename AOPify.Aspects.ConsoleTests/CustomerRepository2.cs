using AOPify.Aspects.AspectAttributes;
using AOPify.Aspects.Base;
using AOPify.Aspects.Enum;

namespace AOPify.Aspects.ConsoleTests
{
    [AOPify(ProcessMode.All, typeof(ConsoleProcessor))]
    public class CustomerRepository2 : AspectObject
    {
        public int Count
        {
            get;
            set;
        }

        public Customer GetCustomerById(int id)
        {
            return new Customer(id);
        }

    }
}