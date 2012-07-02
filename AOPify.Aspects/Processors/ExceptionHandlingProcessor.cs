using System;
using System.Runtime.Remoting.Messaging;
using AOPify.Aspects.Contexts;
using AOPify.Aspects.IProcessor;

namespace AOPify.Aspects.Processors
{
    public abstract class ExceptionHandlingAspectProcessor : IPostAspectProcessor
    {
        public void Process(MethodCallContext callContext, ref MethodReturnContext returnContext)
        {
            Exception exception = returnContext.Exception;

            if (exception != null)
            {
                HandleException(exception);

                Exception newException = GetNewException(exception);

                if (!ReferenceEquals(exception, newException))
                {
                    returnContext =
                        (IMethodReturnMessage)new ReturnMessage(newException, callContext) as MethodReturnContext;
                }
            }
        }

        public abstract void HandleException(Exception exception);

        public virtual Exception GetNewException(Exception oldException)
        {
            return oldException;
        }

        #region Implementation of ILoggable

        public void Log(string message)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
