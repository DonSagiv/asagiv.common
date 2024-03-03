using System;

namespace asagiv.Appl.Core.Interfaces
{
    public interface ILogAppenderService
    {
        ILogAppender GetLogger(string name);
        ILogAppender GetLogger(Type type);
        ILogAppender GetLogger<T>();
    }
}
