using System;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Emily.Clock.UnitTests.Mocks;

internal class LoggerMock: ILogger
{
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log(LogLevel logLevel, EventId eventId, string state, Exception exception, MethodInfo format)
    {
        LogCalled = true;
    }

    public bool LogCalled { get; set; }

}