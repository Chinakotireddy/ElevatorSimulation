using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using ElevatorSimulation.Configuration;


namespace ElevatorSimulation.ElevatorLogger
{
    public static class ELogger
    {
        static readonly ILogger logger = null;

        /// <summary>
        /// This Method returns the Debeg level based on the Environment  
        /// Priority order of Logs => Critical > Error > Warning > Information > Debug > Trace 
        /// </summary>
        /// <returns>Returns the Debeg level</returns>
        private static LogLevel GetLogLevelBasedOnEnvironment() =>
            ElevatorAppConfig.DeploymentEnvironment switch
            {
                "Development" => LogLevel.Debug,
                "Staging" => LogLevel.Information,
                "Production" => LogLevel.Warning,
                _ => LogLevel.Warning
            };

        /// <summary>
        /// This is the Construct Mehod that prepare the logger object which can log 
        /// the debug prints while application is running
        /// </summary>
        static ELogger()
        {
            LogLevel logLevel = GetLogLevelBasedOnEnvironment();
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", logLevel)
                    .AddFilter("System", logLevel)
                    .AddFilter("LoggingConsoleApp.Program", logLevel)
                    .AddFilter("NonHostConsoleApp.Program", logLevel)
                    .AddConsole();
            });
            logger = loggerFactory.CreateLogger<Program>();
        }

        /// <summary>
        /// This Method returns the logger object
        /// </summary>
        /// <returns>Returns the logger object</returns>
        public static ILogger GetLogger()
        {
            return logger;
        }

    }
}
