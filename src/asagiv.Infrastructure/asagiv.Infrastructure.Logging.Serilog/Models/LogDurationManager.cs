using asagiv.Appl.Core.Enumerators;
using asagiv.Appl.Core.Interfaces;
using System;
using System.Diagnostics;

namespace asagiv.Infrastructure.logging.Models
{
    public class LogDurationManager : IDisposable
    {
        #region Fields
        private readonly ILogAppender _appender;
        private readonly LogLevel logLevel;
        private readonly Stopwatch _sw;
        private readonly string _message;
        #endregion

        public LogDurationManager(ILogAppender appender, LogLevel logLevel, string message, params object[] args)
        {
            _appender = appender;

            _message = string.Format(message, args);

            appender.Append(logLevel, "{0}: started.", _message);

            _sw = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _sw.Stop();

            var elapsedTime = _sw.ElapsedMilliseconds;

            _appender.Append(logLevel, "{0}: completed, {1} ms.", _message, elapsedTime);
        }
    }
}
