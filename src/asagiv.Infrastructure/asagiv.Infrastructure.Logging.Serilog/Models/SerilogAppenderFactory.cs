using asagiv.Appl.Core.Interfaces;
using System;

namespace asagiv.Infrastructure.logging.serilog.Models
{
    public class SerilogAppenderFactory : ILogAppenderService
    {
        #region Methods
        public ILogAppender GetLogger(string name)
        {
            return new SerilogAppender(name);
        }

        public ILogAppender GetLogger(Type type)
        {
            return GetLogger(nameof(type));
        }

        public ILogAppender GetLogger<T>()
        {
            return GetLogger(nameof(T));
        }
        #endregion
    }
}
