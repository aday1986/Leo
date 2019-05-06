using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Logging.EF
{
    public class EFLoggerProvider : ILoggerProvider
    {
        private readonly ILogService logService;
        private readonly IConfiguration configuration;
        public const string ProviderName = "EF";

        public EFLoggerProvider(ILogService logService,IConfiguration configuration=null)
        {
            this.logService = logService;
            this.configuration = configuration;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new EFLogger(categoryName, logService, configuration);
        }

        public void Dispose()
        {
            this.Dispose();
            //throw new NotImplementedException();
        }

       
    }
}
