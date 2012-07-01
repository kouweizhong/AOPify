using System;
using AOPify.Aspects.Common;
using AOPify.Aspects.Contexts;
using AOPify.Aspects.IProcessor;

namespace AOPify.Aspects.Processors
{
    public class ExecutionTimeProcessor : IPreAspectProcessor, IPostAspectProcessor
    {
        private ExecutionTimer _timer;
        public string ExecutionMessage { get; set; }

        #region IPreAspectProcessor Members

        public void Process(ref MethodCallContext callContext)
        {
            _timer = new ExecutionTimer();
            callContext.Properties.Add("codeTimer", _timer);
            _timer.Start(callContext.MethodName);
        }

        #endregion

        #region IPostAspectProcessor Members

        public void Process(MethodCallContext callContext, ref MethodReturnContext returnContext)
        {
            _timer = (ExecutionTimer)callContext.Properties["codeTimer"];
            _timer.Finish();
            TimeSpan ts = _timer.StartDate.Subtract(_timer.EndDate);
            ExecutionMessage = "Total time for {0}:{1}ms".FormatWith(_timer.Operation, ts.TotalMilliseconds);
        }

        #endregion

        #region Implementation of ILoggable

        public virtual void Log(string message)
        {
            Console.WriteLine(ExecutionMessage);
        }

        #endregion
    }
}
