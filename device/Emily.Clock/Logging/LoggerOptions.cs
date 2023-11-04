using Microsoft.Extensions.Logging;

namespace Emily.Clock.Logging
{
    public class LoggerOptions
    {
        public LogLevel MinLogLevel { get; }

        public LoggerOptions(LogLevel minLogLevel)
        {
            MinLogLevel = minLogLevel;
        }
    }
}
