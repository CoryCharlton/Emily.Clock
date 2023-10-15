using Microsoft.Extensions.Logging;

namespace Emily.Clock.Logging
{
    /// <summary>
    /// Provides a simple Debugger logger
    /// </summary>
    public class DebugLoggerFactory : ILoggerFactory
    {
        private readonly LoggerOptions _loggerOptions;

        public DebugLoggerFactory(LoggerOptions loggerOptions)
        {
            _loggerOptions = loggerOptions;
        }

        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName)
        {
            return DebugLogger.Create(categoryName, _loggerOptions);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // nothing to do here
        }
    }
}
