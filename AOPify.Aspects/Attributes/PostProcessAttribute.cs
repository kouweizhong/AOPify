using System;
using AOPify.Aspects.Enum;
using AOPify.Aspects.IProcessor;

namespace AOPify.Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class PostProcessAttribute : Attribute
    {
        private readonly PostProcessMode[] _processModes;
        private readonly IPostAspectProcessor _aspectProcessor;
        public PostProcessAttribute(Type postProcessorType, params PostProcessMode[] processModes)
        {
            _processModes = processModes;
            _aspectProcessor = Activator.CreateInstance(postProcessorType) as IPostAspectProcessor;

            if (_aspectProcessor == null)
            {
                throw new ArgumentException(
                    String.Format("The type '{0}' does not implement interface IPostAspectProcessor", postProcessorType.Name));
            }
        }

        public IPostAspectProcessor AspectProcessor
        {
            get { return _aspectProcessor; }
        }

        public PostProcessMode[] GetProcessModes()
        {
            return _processModes;
        }
    }
}
