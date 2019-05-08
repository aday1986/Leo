using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Logging.Sqlite
{
    public class SqliteLoggerProvider : ILoggerProvider
    {
        private readonly Data.IRepository<LogInfo> repository;
        private readonly IConfiguration configuration;
        public const string ProviderName = "Sqlite";

        public SqliteLoggerProvider(Leo.Data.IRepository<LogInfo>  repository,IConfiguration configuration=null)
        {
            this.repository = repository;
            this.configuration = configuration;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new SqliteLogger(categoryName, repository, configuration);
        }

        public void Dispose()
        {
            this.Dispose();
        }

       
    }
}
