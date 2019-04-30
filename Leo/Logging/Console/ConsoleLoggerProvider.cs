using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Logging.Console
{
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        private readonly LoggerFilterOptions options;

        public ConsoleLoggerProvider(LoggerFilterOptions options = null)
        {
            this.options = options;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new ConsoleLogger(categoryName,options);
        }

        public const string ProviderName = "Console";
      

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
