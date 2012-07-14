using System;
using AOPify.Aspects.AspectAttributes;
using AOPify.Aspects.Attributes;
using AOPify.Aspects.Base;
using AOPify.Aspects.Enum;

namespace AOPify.Aspects.ConsoleTests
{
    [AOPifyAttribute]
    public class CustomerRepository : AspectObject
    {
        public int Count
        {
            get;
            set;
        }

        [PreProcess(typeof(ConsolePreAspectProcessor), PreProcessMode.OnBefore, PreProcessMode.WithInputParameters)]
        [PostProcess(typeof(ConsolePostAspectProcessor), PostProcessMode.OnAfter, PostProcessMode.WithReturnType, PostProcessMode.OnError, PostProcessMode.HowLong)]
        public Customer GetCustomerByID(int id)
        {
            return new Customer(id);
        }

        [PreProcess(typeof(ConsolePreAspectProcessor), PreProcessMode.OnBefore, PreProcessMode.WithInputParameters)]
        [PostProcess(typeof(ConsolePostAspectProcessor), PostProcessMode.OnAfter, PostProcessMode.OnError, PostProcessMode.HowLong)]
        public Customer ThrowException()
        {
            //Use hybrid
            AOPify.Let
               .Catch(exception => Console.WriteLine(exception.Message))
               .Run(() =>
               {
                   Console.WriteLine("Before Exception");
                   throw new ApplicationException("An error");
               });

            return null;
        }
    }
}
