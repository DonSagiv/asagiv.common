using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace asagiv.common.Logging
{
    public static class LoggerFactory
    {
        #region Statics
        private const string consoleOutputTemplate = "{Level:u} {Timestamp:yyyy-MM-dd hh:mm:ss.fff tt} [{ThreadId}] {Message}{NewLine}{Exception}";
        private const string jsonOutputTemplate = "{ Level : {Level:u}, TimeStamp : {Timestamp:yyyy-MM-dd hh:mm:ss.fff tt}, Thread : {ThreadId}, Message : {Message}, Exception : {Exception}},{NewLine}";
        #endregion

        #region Methods
        public static void UseSerilog(this IServiceCollection serviceCollection)
        {
            var loggerConfiguration = InitializeConfig()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: consoleOutputTemplate)
                .WriteTo.File(new JsonFormatter(), "Logs/log-.json", rollingInterval: RollingInterval.Day);

            serviceCollection.AddSingleton<ILogger>(loggerConfiguration.CreateLogger());
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