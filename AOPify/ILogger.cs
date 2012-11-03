using System;

namespace AOPify
{
    public interface ILogger
    {
        void Log(string format, params object[] args);
        void Error(Exception exception);
        void Error(Exception exception,string message);
        void Info(string format, params object[] args);
    }
}
