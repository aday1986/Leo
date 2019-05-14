using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Logging.File
{
    public class FileLoggerProvider : ILoggerProvider
    {

        public FileLoggerProvider(string dir, IConfiguration configuration = null)
        {
            this.dir = dir;
            this.configuration = configuration;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(dir, categoryName,configuration);
        }

        public const string ProviderName = "File";
        private readonly string dir;
        private readonly IConfiguration configuration;

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
