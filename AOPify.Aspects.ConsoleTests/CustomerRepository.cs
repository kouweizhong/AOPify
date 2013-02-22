using AOPify.Aspects.AspectAttributes;
using AOPify.Aspects.Attributes;
using AOPify.Aspects.Base;

namespace AOPify.Aspects.ConsoleTests
{
    [AOPify]
    public class CustomerRepository : AspectObject
    {
        public int Count
        {
            get;
            set;
        }

        [PreProcess(typeof(ConsolePreProcessor))]
        [PostProcess(typeof(ConsolePostProcessor))]
        public Customer GetCustomerById(int id)
        {
            return new Customer(id);
        }
    }
}
