using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Leo.Logging.EF
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEFLogging(this IServiceCollection services)
        {
            string dir = $"{AppDomain.CurrentDomain.BaseDirectory }Logging";
            string path = $"Filename={dir}/log.db";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return services.AddEFLogging(path);
        }

        public static IServiceCollection AddEFLogging(this IServiceCollection services, string path)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddDbContext<LogContext>(options => options
                .UseSqlite(path)
                .UseLoggerFactory(new LoggerFactory())
                , ServiceLifetime.Singleton)
                .AddSingleton<ILogService, EFLogService>();
            services.AddLogging(bulder => bulder.AddProvider(
                new EFLoggerProvider(services.BuildServiceProvider().GetService<ILogService>(), configuration))
);
            var db = services.BuildServiceProvider().GetService<LogContext>();
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
            return services;
        }



    }
}
