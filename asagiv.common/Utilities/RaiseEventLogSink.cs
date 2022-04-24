using System;
using Serilog.Core;
using Serilog.Events;

namespace asagiv.common.Utilities
{
    public class RaiseEventLogSink : ILogEventSink
    {
        #region Fields
        private readonly IFormatProvider? _formatProvider;
        #endregion

        #region Delegates
        public event EventHandler<string>? LogEventRaised;
        #endregion

        #region Constructor
        public RaiseEventLogSink(IFormatProvider? formatProvider = null)
        {
            _formatProvider = formatProvider;
        }
        #endregion

        public void Emit(LogEvent logEvent)
        {
            if(_formatProvider == null)
            {
                return;
            }

            var logEntry = logEvent.RenderMessage(_formatProvider);
            LogEventRaised?.Invoke(this, logEntry);
        }
    }
}
