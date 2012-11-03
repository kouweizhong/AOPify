using System;

namespace AOPify.NLog
{
    public class NLogger : ILogger
    {
        public void Log(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception exception, string message)
        {
            throw new NotImplementedException();
        }

        public void Info(string format, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}