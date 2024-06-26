using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.Generic.Logging
{
	public class LoggerHelper
	{
		private ILogger _logger;
		private EventId eventId = new EventId();

		public LoggerHelper(ILoggerProvider loggerProvider)
		{
			this._logger = loggerProvider.CreateLogger("Global");
		}

		public void Log<TState>(LogLevel logLevel, TState state, Exception? exception)
		{
			if (_logger.IsEnabled(logLevel))
			{
				_logger.Log(logLevel: logLevel, eventId: eventId, state: state, exception: exception, formatter: null);
			}
		}
	}
}
