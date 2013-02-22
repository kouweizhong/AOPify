using System;
using AOPify.Aspects.Interface;

namespace AOPify.Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class PostProcessAttribute : Attribute
    {

        private readonly IPostAspectProcessor _aspectProcessor;
        public PostProcessAttribute(Type postProcessorType)
        {
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
    }
}
