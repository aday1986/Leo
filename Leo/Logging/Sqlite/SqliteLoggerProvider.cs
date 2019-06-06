using Leo.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Logging.Sqlite
{
    public class SqliteLoggerProvider : ILoggerProvider
    {
        private readonly IDML dml;
        private readonly IConfiguration configuration;
        public const string ProviderName = "Sqlite";

        public SqliteLoggerProvider(IDML  dml,IConfiguration configuration=null)
        {
            this.dml = dml;
            this.configuration = configuration;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new SqliteLogger(categoryName, dml, configuration);
        }

        public void Dispose()
        {
            this.Dispose();
        }

       
    }
}
