using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Logging.Console
{
    public class ConsoleLoggerProvider : ILoggerProvider
    {

        public ConsoleLoggerProvider(IConfiguration configuration = null)
        {
            this.configuration = configuration;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new ConsoleLogger(categoryName,configuration);
        }

        public const string ProviderName = "Console";
        private readonly IConfiguration configuration;

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
