using System;
using AOPify.Aspects.Contexts;
using AOPify.Aspects.Interface;
using AOPify.Aspects.Sinks;

namespace AOPify.Aspects.Processors
{
    public abstract class ExceptionHandlingAspectProcessor : IPostAspectProcessor
    {
        public void Process(PostProcessContext preProcessContext)
        {
            throw new NotImplementedException();
        }

        public virtual void OnAfter(MethodCallContext callContext)
        {
            
        }

        public virtual void OnError(Exception exception)
        {
        }

        public virtual void WithReturnType()
        {
           
        }

        public void HowLong(string operation, int milliseconds)
        {
            
        }

        public abstract void HandleException(Exception exception);

        public virtual Exception GetNewException(Exception oldException)
        {
            return oldException;
        }
    }
}