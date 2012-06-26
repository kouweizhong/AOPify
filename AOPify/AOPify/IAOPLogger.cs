using System;

namespace AOPify
{
    public interface IAOPLogger
    {
        void Log(string format, params object[] args);
        void Error(Exception exception);
        void Error(Exception exception,string message);
        void Info(string format, params object[] args);
    }
}
