using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Leo.Data;
using Leo.Data.Sqlite;
using Leo.Data.Core;

namespace Leo.Logging.Sqlite
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqliteLogging(this IServiceCollection services)
        {
            string dir = $"{AppDomain.CurrentDomain.BaseDirectory }\\data\\log";
            string path = $"DataSource={dir}\\log.db";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return services.AddSqliteLogging(path);
        }

        public static IServiceCollection AddSqliteLogging(this IServiceCollection services, string path)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            //单独注入一个仓储
            SQLitePCL.Batteries.Init();
            IServiceCollection logservices = new ServiceCollection();
            logservices.AddRepository(new SqliteDbProvider(path));
            using (var db = logservices.BuildServiceProvider().GetService<IDbProvider>().CreateConnection())
            {
                db.Open();
                var cmd = db.CreateCommand();
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS main.LogInfo(" +
               "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
               "CreateTime  TEXT NOT NULL,LogLevel  TEXT," +
               "Message  TEXT," +
               "EventId  INTEGER NOT NULL," +
               "EventName  TEXT); ";
                cmd.ExecuteNonQuery();
            }
            services.AddLogging(bulder => bulder.AddProvider(
                new SqliteLoggerProvider(logservices.BuildServiceProvider().GetService<IDML>(), configuration)));
            return services;
        }
    }
}
