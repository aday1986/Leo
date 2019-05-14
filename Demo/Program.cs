using Leo.Account;
using Leo.Config;
using Leo.Data;
using Leo.Data.Dapper;
using Leo.Data.EF;
using Leo.Fac;
using Leo.Logging.Console;
using Leo.Logging.Sqlite;
using Leo.ThirdParty.AutoMapper;
using Leo.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Leo.Logging.File;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceProvider provider = ConfigureServices();
            var repository = provider.GetService<IRepository<UserInfo>>();
            while (true)
            {
                Stopwatch stopwatch = new Stopwatch();
                Console.WriteLine("1.add;2.query,3.log");
                switch (Console.ReadLine())
                {
                    case "log":
                        Stopwatch logWatch = new Stopwatch();
                        logWatch.Start();
                        var logger = provider.GetService<ILogger<Program>>();
                        for (int t = 0; t < 100; t++)
                        {
                            Thread thread = new Thread(() =>
                            {
                                int t1 = t;//闭包
                                for (int i = 0; i < 100; i++)
                                {
                                    //Thread.Sleep(random.Next(1,2000));
                                    logger.LogWarning($"{t1}-{i}");
                                }
                                lock (logWatch)
                                {
                                    logWatch.Stop();
                                    Console.WriteLine($"进程{t1}共用时{logWatch.Elapsed.TotalSeconds}秒。");
                                    logWatch.Start();
                                }

                            });
                            thread.Start();
                        }
                        break;

                    case "add":

                        stopwatch.Start();
                        double maxN = 10;
                        double maxM = 10;
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

                        break;
                    case "query":
                        var r = repository.Query(new[] { new Condition() { ConditionType = ConditionEnum.NotEqual, Key = "Guid", Value = string.Empty } });
                        Console.WriteLine(r.Count());
                        break;
                    default:
                        break;
                }
                stopwatch.Stop();
             if(stopwatch.Elapsed.TotalSeconds>0) Console.WriteLine($"共用时{stopwatch.Elapsed.TotalSeconds}秒。");
            }
        }

        public static IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddConfiguration();
            services.AddEFRepository(new EntityTypeCollection(new[] { typeof(UserInfo) }) , option => option.UseSqlite("Filename=data.db"));
            //services.AddEFRepository(new EntityTypeProvider(new[] { typeof(UserInfo) }), option => option.UseInMemoryDatabase("data"));
            //services.AddDapperRepository(new SqliteDbProvider($"Data Source={AppDomain.CurrentDomain.BaseDirectory}data.db"));
          
            services.AddSqliteLogging();
            services.AddFileLogging();
            services.AddConsole();
            var r = Assembly.GetEntryAssembly().GetAllAssemblies();
            var types = Assembly.GetEntryAssembly().GetAllDefinedTypes();

            IServiceProvider provider = services.BuildServiceProvider();
#if DEBUG
            //services.AddLogging(builder => builder.AddDebug());
#endif
            return provider;
        }
    }





}
