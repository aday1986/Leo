using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Logging.File
{
    public class FileLoggerProvider : ILoggerProvider
    {

        public FileLoggerProvider(IConfiguration configuration = null)
        {
            this.configuration = configuration;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName,configuration);
        }

        public const string ProviderName = "File";
        private readonly IConfiguration configuration;

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
