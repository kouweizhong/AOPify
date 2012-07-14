using System;
using AOPify.Aspects.Contexts;
using AOPify.Aspects.IProcessor;

namespace AOPify.Aspects.ConsoleTests
{
    public class ConsoleProcessor : IPreAspectProcessor, IPostAspectProcessor
    {
        #region Implementation of IPreAspectProcessor

        public void Process(ref MethodCallContext callContext)
        {

        }

        #endregion

        #region Implementation of IPostAspectProcessor

        public void Process(MethodCallContext callContext, ref MethodReturnContext returnContext)
        {

        }

        #endregion


        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}