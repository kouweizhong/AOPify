using System;
using AOPify.Aspects.IProcessor;
using AOPify.Enum;

namespace AOPify.Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class PreProcessAttribute : Attribute
    {
        private readonly PreProcessMode[] _processModes;
        private readonly IPreAspectProcessor _aspectProcessor;

        public PreProcessAttribute(Type preProcessorType, params PreProcessMode[] processModes)
        {
            _processModes = processModes;
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

        public PreProcessMode[] GetProcessModes()
        {
            return _processModes;
        }
    }
}
