using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Leo.Account;
using Leo.Data;
using Leo.Data.Dapper;
using Leo.Data.EF;
using Leo.Fac;
using Leo.Logging.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            var repository = services.BuildServiceProvider().GetService<IRepository<UserInfo>>();
            while (true)
            {
                Console.WriteLine("1.add;2.query");
                switch (Console.ReadLine())
                {
                    case "add":
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        double maxN = 100;
                        double maxM = 1;
                        for (int n = 1; n <= maxN; n++)
                        {
                            List<UserInfo> models = new List<UserInfo>();
                            for (int m = 1; m <= maxM; m++)
                            {
                                models.Add(new UserInfo() { Guid = Guid.NewGuid().ToString(), UserName = Guid.NewGuid().ToString() });
                            }
                            repository.AddRange(models);
                           repository.SaveChanges();
                            //Console.Write($"\r{((n / maxN) * 100).ToString("0.0") }%");
                        }
                        stopwatch.Stop();
                        Console.WriteLine($"完成{maxN*maxM}，共用时{stopwatch.Elapsed.TotalSeconds}秒。");
                        break;
                    case "query":
                        var r = repository.Query(new[] { new Condition() { ConditionType = ConditionEnum.NotEqual, Key = "Guid", Value = string.Empty } });
                        Console.WriteLine(r.Count());
                        break;
                    default:
                        break;
                }
            }
        }

        public static void ConfigureServices(IServiceCollection services)
        {
           
            //services.AddEFRepository(new EntityTypeProvider(new[] { typeof(UserInfo) }) , option => option.UseSqlite("Filename=data.db"));
            //services.AddEFRepository(new EntityTypeProvider(new[] { typeof(UserInfo) }), option => option.UseInMemoryDatabase("data"));
            services.AddDapperRepository(new SqliteDbProvider($"Data Source={AppDomain.CurrentDomain.BaseDirectory}data.db"));
            services.AddLogging(builder => builder.AddProvider(new Leo.Logging.Console.ConsoleLoggerProvider()));
            services.AddEFLogging(null);
#if DEBUG
            services.AddLogging(builder =>builder.AddDebug());
#endif
        }


    }
}
