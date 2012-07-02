using System;

namespace AOPify.Aspects.Processors
{
	public class ChangeExceptionAspectProcessor : ExceptionHandlingAspectProcessor
	{
	    public override void HandleException(Exception exception)
		{			
		}
	
		public override Exception GetNewException(Exception oldException)
		{
			return new ApplicationException("Another one");
		}
	}
}
