using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.NSGHSM.Core.Test._Mockups
{
    public class LoggerMock<T> : ILogger<T>
    {
        public Func<object, IDisposable> BeginScopeCallback { get; set; }

        public IDisposable BeginScope<TState>(TState state)
        {
            return BeginScopeCallback?.Invoke(state);
        }

        public Func<bool> IsEnabledCallback { get; set; }

        public bool IsEnabled(LogLevel logLevel)
        {
            return IsEnabledCallback?.Invoke() ?? true;
        }

        public Action<LogLevel, EventId, object, Exception> LogCallback { get; set; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LogCallback?.Invoke(logLevel, eventId, state, exception);
        }
    }
}
