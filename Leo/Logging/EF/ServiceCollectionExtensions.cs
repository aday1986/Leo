using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;


namespace Leo.Logging.EF
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEFLogging(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            services.AddDbContext<LogContext>(options => options
                .UseSqlite($"Filename={AppDomain.CurrentDomain.BaseDirectory}Logging/log.db")
                .UseLoggerFactory(new LoggerFactory())
                , ServiceLifetime.Singleton)
                .AddSingleton<ILogService, EFLogService>();
            services.AddLogging(bulder =>bulder.AddProvider(
                new EFLoggerProvider(services.BuildServiceProvider().GetService<ILogService>(),configuration))
);
            var db = services.BuildServiceProvider().GetService<LogContext>();
            if (db.Database.EnsureCreated())//初始化数据库。
            {
                db.Database.ExecuteSqlCommand(new RawSqlString("DROP TABLE IF EXISTS main.LogInfo;" +
                "CREATE TABLE LogInfo(" +
                "Id  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                "CreateTime  TEXT NOT NULL,LogLevel  TEXT," +
                "Message  TEXT," +
                "EventId  INTEGER NOT NULL," +
                "EventName  TEXT); "));
            } 
            services.BuildServiceProvider().GetService<LogContext>().Database.Migrate();
            return services;
        }


      
    }
}
