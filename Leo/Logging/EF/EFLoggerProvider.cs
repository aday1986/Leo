using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Logging.EF
{
    public class EFLoggerProvider : ILoggerProvider
    {
        private readonly ILogService logService;
        private readonly LoggerFilterOptions options;
        public const string ProviderName = "EF";

        public EFLoggerProvider(ILogService logService,LoggerFilterOptions  options=null)
        {
            this.logService = logService;
            this.options = options;
        }
        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return new EFLogger(categoryName, logService, options);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

       
    }
}
