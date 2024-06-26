using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.Generic.Logging
{
    public class TCGCTLogger : ILogger
    {
        public static LogLevel currentLogLevel = LogLevel.Trace;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            // not sure what im supposed to do with this
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= currentLogLevel; 
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
			Console.WriteLine($"{DateTime.Now} | {logLevel} | {state}{exception}");
        }
    }
}
