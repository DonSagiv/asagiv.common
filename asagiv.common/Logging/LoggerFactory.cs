using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace asagiv.common.Logging
{
    public static class LoggerFactory
    {
        #region Statics
        private const string consoleOutputTemplate = "{Level:u} {Timestamp:yyyy-MM-dd hh:mm:ss.fff tt} [{ThreadId}] {Message}{NewLine}{Exception}";
        private const string defaultLogPath = "Logs/log-.json";
        #endregion

        #region Methods
        public static void UseSerilog(this IServiceCollection serviceCollection, string loggerDirectory = null, bool useDebug = false)
        {
            serviceCollection.AddSingleton(CreateLogger(loggerDirectory));
        }

        public static ILogger CreateLogger(string loggerDirectory = null)
        {
            var loggerConfiguration = InitializeConfig()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: consoleOutputTemplate)
                .WriteTo.Debug(restrictedToMinimumLevel: LogEventLevel.Debug, outputTemplate: consoleOutputTemplate);

            var logSaveDirectory = loggerDirectory == null
                ? defaultLogPath
                : Path.Combine(loggerDirectory, defaultLogPath);

            loggerConfiguration = loggerConfiguration
                .WriteTo.File(new JsonFormatter(), logSaveDirectory, rollingInterval: RollingInterval.Day);

#if DEBUG
            SelfLog.Enable(msg => Debug.WriteLine(msg));
            SelfLog.Enable(Console.Error);
#endif

            var logger = loggerConfiguration.CreateLogger();

            logger.Information("Logger Initialized.");

            return logger;
        }

        private static LoggerConfiguration InitializeConfig()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithEnvironmentName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId();
        }
        #endregion
    }
}