using System;
using Microsoft.Extensions.Logging;

namespace DA.WI.NSGHSM.Core.Services
{
    public class ConsoleLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine($"{DateTime.Now}\t{logLevel}\t{formatter(state, exception)}");
        }
    }
}