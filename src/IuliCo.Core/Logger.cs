using System;
using System.IO;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace IuliCo.Core
{
    public class AsyncLogger
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private static readonly AsyncLogger instance = new AsyncLogger();

        // Private constructor to prevent external instantiation
        private AsyncLogger()
        {
            var config = new LoggingConfiguration();
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            // Ensure the directory exists
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            // Create targets
            var logfile = new FileTarget("logfile")
            {
                // filename will contain the date of the day
                FileName = Path.Combine(logPath, "log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt"),
                Layout = "${longdate} ${level:uppercase=true} ${message} ${exception}"
            };
            var logconsole = new ColoredConsoleTarget("logconsole")
            {
                Layout = "${longdate} ${level:uppercase=true} ${message} ${exception}"
            };
            // Define color mappings for each log level
            logconsole.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Trace", ConsoleOutputColor.Gray, ConsoleOutputColor.NoChange));
            logconsole.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Debug", ConsoleOutputColor.White, ConsoleOutputColor.NoChange));
            logconsole.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Info", ConsoleOutputColor.DarkBlue, ConsoleOutputColor.NoChange));
            logconsole.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Warn", ConsoleOutputColor.DarkYellow, ConsoleOutputColor.NoChange));
            logconsole.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Error", ConsoleOutputColor.Magenta, ConsoleOutputColor.NoChange));
            logconsole.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Fatal", ConsoleOutputColor.DarkMagenta, ConsoleOutputColor.NoChange));

            // Add rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);

            LogManager.Configuration = config;
        }

        public static AsyncLogger Instance => instance;

        public async Task LogAsync(IuliCo.Core.Enums.LogLevel logLevel, string message)
        {
            await Task.Run(() => Log(logLevel, message));
        }

        private void Log(IuliCo.Core.Enums.LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case IuliCo.Core.Enums.LogLevel.Trace:
                    logger.Trace(message);
                    break;
                case IuliCo.Core.Enums.LogLevel.Debug:
                    logger.Debug(message);
                    break;
                case IuliCo.Core.Enums.LogLevel.Info:
                    logger.Info(message);
                    break;
                case IuliCo.Core.Enums.LogLevel.Warn:
                    logger.Warn(message);
                    break;
                case IuliCo.Core.Enums.LogLevel.Error:
                    logger.Error(message);
                    break;
                case IuliCo.Core.Enums.LogLevel.Fatal:
                    logger.Fatal(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }
    }
}
