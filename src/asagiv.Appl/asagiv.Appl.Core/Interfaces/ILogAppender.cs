using asagiv.Appl.Core.Enumerators;
using System;

namespace asagiv.Appl.Core.Interfaces
{
    public interface ILogAppender
    {
        void Verbose(string message, params object[] args);
        void Verbose(Exception exception, string message = null, params object[] args);
        void Debug(string message, params object[] args);
        void Debug(Exception exception, string message = null, params object[] args);
        void Info(string message, params object[] args);
        void Info(Exception exception, string message = null, params object[] args);
        void Warn(string message, params object[] args);
        void Warn(Exception exception, string message = null, params object[] args);
        void Error(string message, params object[] args);
        void Error(Exception exception, string message = null, params object[] args);
        void Append(LogLevel level, string message, params object[] args);
        void Append(LogLevel level, Exception exception, string message = null, params object[] args);
        IDisposable Duration(string message, LogLevel level = LogLevel.Default, params object[] args);
    }
}
