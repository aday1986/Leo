﻿using ConsoleApp1.Models;
using Leo.Config;
using Leo.Data;
using Leo.Data.Expressions;
using Leo.Data.Sqlite;
using Leo.Logging.Console;
using Leo.Logging.File;
using Leo.Logging.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Demo
{
    class Program
    {
         static void Main(string[] args)
        {
            var services = ConfigureServices();
           var repository = services.GetService<IRepository<Students>>();
               
            while (true)
            {
                Random random = new Random();
                Stopwatch stopwatch = new Stopwatch();
                Console.WriteLine("1.add;2.query,3.get,4.remove");
                var key = Console.ReadLine();
                stopwatch.Start();
                switch (key)
                {
                    case "add":
                    case "1":
                        List<Students> students = new List<Students>();
                        for (int i = 0; i < 10000; i++)
                        {
                            students.Add(new Students() { StudentName = Guid.NewGuid().ToString(), CreateDate = DateTime.Now, StudentEnum = (StudentEnum)random.Next(0, 2) });
                        }
                        repository.Add(students.ToArray());
                        Console.WriteLine(repository.SaveChanges());
                        break;
                    case "2":
                    case "query":
                        var result = repository.Query().Where(t => t.Id >= 50 && t.Id <= 100).ToArray();
                       Console.WriteLine(result.Count());
                        break;

                    case "3":
                    case "get":
                        var r = repository.Get(1);
                        break;

                    case "4":
                    case "remove":
                        //repository.Remove(t => t.Id >= 10000);
                        repository.Remove(new Students() { Id = 10000 });
                        Console.WriteLine(repository.SaveChanges());
                        break;

                    default:
                        break;
                }
                stopwatch.Stop();
                if (stopwatch.Elapsed.TotalSeconds > 0) Console.WriteLine($"共用时{stopwatch.Elapsed.TotalSeconds}秒。");
            }
        }

        public static IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddConfiguration();
            services.AddSqliteLogging();
            services.AddFileLogging();
            services.AddConsoleLogging();
            //services.AddEFRepository(new EntityTypeCollection(new[] { typeof(LogInfo) }) , option => option.UseSqlite("Filename=data\\log\\log.db"));
            //services.AddEFRepository(new EntityTypeCollection(new[] { typeof(LogInfo) }), option => option.UseInMemoryDatabase("data"));
            services.AddRepository(new SqliteDbProvider($"Data Source={AppDomain.CurrentDomain.BaseDirectory}TestData.db"));
            IServiceProvider provider = services.BuildServiceProvider();
#if DEBUG
            //services.AddLogging(builder => builder.AddDebug());
#endif
            return provider;
        }
    }





}
