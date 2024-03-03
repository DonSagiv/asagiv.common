using asagiv.Appl.Core.Enumerators;
using asagiv.Appl.Core.Interfaces;
using asagiv.Infrastructure.logging.Models;
using Serilog;
using Serilog.Core.Enrichers;
using Serilog.Events;
using Serilog.Formatting.Json;
using System;
using System.IO;

namespace asagiv.Infrastructure.logging.serilog.Models
{
    public class SerilogAppender : ILogAppender
    {
        #region Statics
        private const string consoleOutputTemplate = "{ClassContext}: {Level:u} {Timestamp:yyyy-MM-dd hh:mm:ss.fff tt} [{ThreadId}] {Message}{NewLine}{Exception}";
        private const string defaultLogPath = "log-.json";
        #endregion

        #region Fields
        private readonly ILogger _serilogAppender;
        #endregion

        #region Constructor
        public SerilogAppender(string typeString, string logFileDirectory = null)
        {
            var classContextEnricher = new PropertyEnricher("ClassContext", typeString);

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.With(classContextEnricher)
                .Enrich.WithEnvironmentName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: consoleOutputTemplate)
                .WriteTo.Debug(restrictedToMinimumLevel: LogEventLevel.Debug, outputTemplate: consoleOutputTemplate);

            if (!Directory.Exists(logFileDirectory))
            {
                return;
            }

            var logFilePath = Path.Combine(logFileDirectory, defaultLogPath);

            loggerConfiguration.WriteTo.File(new JsonFormatter(), logFilePath, rollingInterval: RollingInterval.Day);
        }
        #endregion

        #region Methods
        public void Verbose(string message, params object[] args)
        {
            _serilogAppender.Verbose(message, args);
        }

        public void Verbose(Exception exception, string message = null, params object[] args)
        {
            message = message ?? exception.Message;

            _serilogAppender.Verbose(exception, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            _serilogAppender.Debug(message, args);
        }

        public void Debug(Exception exception, string message = null, params object[] args)
        {
            message = message ?? exception.Message;

            _serilogAppender.Debug(exception, message, args);
        }

        public void Info(string message, params object[] args)
        {
            _serilogAppender.Information(message, args);
        }

        public void Info(Exception exception, string message = null, params object[] args)
        {
            message = message ?? exception.Message;

            _serilogAppender.Information(exception, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            _serilogAppender.Warning(message, args);
        }

        public void Warn(Exception exception, string message = null, params object[] args)
        {
            message = message ?? exception.Message;

            _serilogAppender.Warning(exception, message, args);
        }

        public void Error(string message, params object[] args)
        {
            _serilogAppender.Error(message, args);
        }

        public void Error(Exception exception, string message = null, params object[] args)
        {
            message = message ?? exception.Message;

            _serilogAppender.Error(exception, message, args);
        }

        public void Append(LogLevel level, string message, params object[] args)
        {
            _serilogAppender.Write(GetLogLevel(level), message, args);
        }

        public void Append(LogLevel level, Exception exception, string message = null, params object[] args)
        {
            message = message ?? exception.Message;

            _serilogAppender.Write(GetLogLevel(level), exception, message, args);
        }

        public IDisposable Duration(string message, LogLevel level = LogLevel.Default, params object[] args)
        {
            return new LogDurationManager(this, level, message, args);
        }

        private LogEventLevel GetLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Default: return LogEventLevel.Information;
                case LogLevel.Verbose: return LogEventLevel.Verbose;
                case LogLevel.Debug: return LogEventLevel.Debug;
                case LogLevel.Info: return LogEventLevel.Information;
                case LogLevel.Warn: return LogEventLevel.Warning;
                case LogLevel.Error: return LogEventLevel.Error;
                default: throw new ArgumentException("Invalid log level type.");
            }
        }
        #endregion
    }
}
