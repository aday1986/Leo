using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Leo.Data.Dapper;
using Leo.Data;

namespace Leo.Logging.Sqlite
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqliteLogging(this IServiceCollection services)
        {
            string dir = $"{AppDomain.CurrentDomain.BaseDirectory }Logging";
            string path = $"DataSource={dir}/log.db";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return services.AddEFLogging(path);
        }

        public static IServiceCollection AddEFLogging(this IServiceCollection services, string path)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            //单独注入一个仓储
            SQLitePCL.Batteries.Init();
            IServiceCollection logservices = new ServiceCollection();
            logservices.AddDapperRepository(new Data.SqliteDbProvider(path))
                .AddDbContext<DbContext>(options => options.UseSqlite(path), ServiceLifetime.Singleton);
            var db = logservices.BuildServiceProvider().GetService<DbContext>();
            if (db.Database.EnsureCreated())//初始化数据库。
            {
                db.Database.ExecuteSqlCommand(new RawSqlString("DROP TABLE IF EXISTS main.LogInfo;" +
                "CREATE TABLE LogInfo(" +
                "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                "CreateTime  TEXT NOT NULL,LogLevel  TEXT," +
                "Message  TEXT," +
                "EventId  INTEGER NOT NULL," +
                "EventName  TEXT); "));
            }

            services.AddLogging(bulder => bulder.AddProvider(
                new SqliteLoggerProvider(logservices.BuildServiceProvider().GetService<IRepository<LogInfo>>(), configuration))
);
           
            return services;
        }



    }
}
