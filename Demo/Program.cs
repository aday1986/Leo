using ConsoleApp1.Models;
using Leo.Config;
using Leo.Data;
using Leo.Data.Expressions;
using Leo.Data.Sqlite;
using Leo.Logging.Console;
using Leo.Logging.File;
using Leo.Logging.Sqlite;
using Leo.ThirdParty.Dapper;
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
           var repository = services.GetService<IDML>();
               
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
                        for (int i = 0; i < 100000; i++)
                        {
                            students.Add(new Students { StudentName = Guid.NewGuid().ToString(), CreateDate = DateTime.Now });
                        }
                        repository.Add(students.ToArray());
                        Console.WriteLine(repository.SaveChanges());
                        break;
                    case "2":
                    case "query":
                        //var child=  services.GetService<Query<Students>>().Select(t =>new { t.Id });
                        var rStudent = services.GetService<Query<Students>>().Where(t=>t.Id>=0)
                            //.Select(t=>new {t.StudentName,t.CreateDate,t.Id,t.StudentEnum })
                            .ToArray();
                           
                        Console.WriteLine(rStudent.Count());
                        break;

                    case "3":
                    case "dapper":
                        System.Data.IDbConnection db = new System.Data.SQLite.SQLiteConnection($"Data Source={AppDomain.CurrentDomain.BaseDirectory}TestData.db");
                        var rDapper = db.Query<Classes>("SELECT * FROM CLASSES WHERE ID>=0");
                        break;

                    //case "4":
                    //    var rStudent = services.GetService<Query<Students>>().Select(t=>)
                    //    break

                    //case "3":
                    //case "get":
                    //    var r = services.GetService<Query<Students>>().Get("a");
                    //    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(r));
                    //    break;

                    //case "4":
                    //case "remove":
                    //    //repository.Remove(t => t.Id >= 10000);
                    //    repository.Remove(new Students() { Id = 10000 });
                    //    Console.WriteLine(repository.SaveChanges());
                    //    break;

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
