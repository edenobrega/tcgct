using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.Generic.Logging
{
	public class TCGCTLoggerProvider : ILoggerProvider
	{
		public ILogger CreateLogger(string categoryName)
		{
			return new TCGCTLogger();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
