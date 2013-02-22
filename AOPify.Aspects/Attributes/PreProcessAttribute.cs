using System;
using AOPify.Aspects.Interface;

namespace AOPify.Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class PreProcessAttribute : Attribute
    {
        private readonly IPreAspectProcessor _aspectProcessor;

        public PreProcessAttribute(Type preProcessorType)
        {
            _aspectProcessor = Activator.CreateInstance(preProcessorType) as IPreAspectProcessor;

            if (_aspectProcessor == null)
            {
                throw new ArgumentException(
                    String.Format("The type '{0}' does not implement interface IPreAspectProcessor",preProcessorType.Name));
            }
        }

        public IPreAspectProcessor AspectProcessor
        {
            get { return _aspectProcessor; }
        }
    }
}
