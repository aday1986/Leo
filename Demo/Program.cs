﻿using Leo.Account;
using Leo.Config;
using Leo.Data;
using Leo.Data.Dapper;
using Leo.Data.EF;
using Leo.Logging.Console;
using Leo.Logging.Sqlite;
using Leo.ThirdParty.AutoMapper;
using Leo.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Leo.Logging.File;
using Microsoft.Extensions.Logging;
using System.Threading;
using Leo.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceProvider provider = ConfigureServices();
            var repository = provider.GetService<IRepository<LogInfo>>();
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
                            List<LogInfo> models = new List<LogInfo>();
                            for (int m = 1; m <= maxM; m++)
                            {
                                models.Add(new LogInfo() { CreateTime = DateTime.Now, Message = $"数据插入测试{n}-{m}" });
                            }
                            repository.AddRange(models);
                            repository.SaveChanges();
                            //Console.Write($"\r{((n / maxN) * 100).ToString("0.0") }%");
                        }

                        break;
                    case "query":
                        var r = repository.Query(new[] { new Condition() { ConditionType = ConditionEnum.NotEqual, Key = "Id", Value = 0 } });
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
            services.AddSqliteLogging();
            services.AddFileLogging();
            services.AddConsole();
            //services.AddEFRepository(new EntityTypeCollection(new[] { typeof(LogInfo) }) , option => option.UseSqlite("Filename=data\\log\\log.db"));
            //services.AddEFRepository(new EntityTypeCollection(new[] { typeof(LogInfo) }), option => option.UseInMemoryDatabase("data"));
            services.AddDapperRepository(new SqliteDbProvider($"Data Source={AppDomain.CurrentDomain.BaseDirectory}data\\log\\log.db"));

           
            IServiceProvider provider = services.BuildServiceProvider();
#if DEBUG
            //services.AddLogging(builder => builder.AddDebug());
#endif
            return provider;
        }
    }





}
