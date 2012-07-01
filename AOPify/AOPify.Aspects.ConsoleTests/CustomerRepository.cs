using System;
using AOPify.Aspects.AspectAttributes;
using AOPify.Aspects.Attributes;
using AOPify.Aspects.Processors;
using AOPify.Base;
using AOPify.Enum;

namespace AOPify.Aspects.ConsoleTests
{
    [AOPifyAttribute]
    public class CustomerRepository : AspectObject
    {
        public int Count
        {
            [PreProcess(typeof(ConsolePreAspectProcessor))]
            get;
            [PreProcess(typeof(ConsolePreAspectProcessor))]
            set;
        }

        [PreProcess(typeof(ConsolePreAspectProcessor),PreProcessMode.OnBefore,PreProcessMode.WithInputParameters)]
        [PostProcess(typeof(ConsoleExceptionAspectProcessor))]
        [PostProcess(typeof(ConsolePostAspectProcessor),PostProcessMode.OnAfter,PostProcessMode.WithReturnType,PostProcessMode.OnError, PostProcessMode.HowLong)]
        public Customer GetCustomerByID(int id)
        {
            return new Customer(id);
        }

        [PreProcess(typeof(ConsolePreAspectProcessor), PreProcessMode.OnBefore, PreProcessMode.WithInputParameters)]
        [PostProcess(typeof(ChangeExceptionAspectProcessor))]
        [PostProcess(typeof(ConsoleExceptionAspectProcessor))]
        public void ThrowException()
        {
            throw new ApplicationException("An error");
        }
    }

    public class Customer
    {
        private readonly int _id;

        public Customer(int id)
        {
            _id = id;
        }

        public int Id
        {
            get { return _id; }
        }
    }
}
