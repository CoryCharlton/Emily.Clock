using Microsoft.Extensions.Logging;
using System;

namespace Emily.Clock.Logging
{
    public static class LoggerFormatter
    {
        public static string Formatter(string loggerName, LogLevel logLevel, EventId eventId, string state, Exception exception)
        {
            var level = logLevel switch
            {
                LogLevel.Trace => "[T]",
                LogLevel.Debug => "[D]",
                LogLevel.Information => "[I]",
                LogLevel.Warning => "[W]",
                LogLevel.Error => "[E]",
                LogLevel.Critical => "[C]",
                LogLevel.None => string.Empty,
                _ => string.Empty,
            };

            var logger = string.IsNullOrEmpty(loggerName) ? string.Empty : $" ({loggerName})";
            var message = exception == null ? state : $"{state} {exception}";

            return $"{level}{logger}: {message}";
        }

        public static void Initialize()
        {
            LoggerExtensions.MessageFormatter = typeof(LoggerFormatter).GetMethod(nameof(Formatter));
        }
    }
}
