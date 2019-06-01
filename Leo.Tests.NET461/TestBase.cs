using Leo.Config;
using Leo.Data;
using Leo.Data.Sqlite;
using Leo.Logging;
using Leo.Logging.Console;
using Leo.Logging.File;
using Leo.Logging.Sqlite;
using Leo.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Leo.Tests.NET461
{
    public abstract class TestBase
    {
        protected static void IsTrue(bool tf)
        {
            Assert.IsTrue(tf);
        }
        protected static IServiceProvider ServiceProvider { get; }

      static TestBase()
        {
            ServiceProvider = ConfigServices();
        }

        private static IServiceProvider ConfigServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddConfiguration();
            services.AddSqliteLogging();
            services.AddFileLogging();
            services.AddConsoleLogging();
            services.AddRepository(new SqliteDbProvider($"Data Source={AppDomain.CurrentDomain.BaseDirectory}\\data\\data.db"));
            IServiceProvider provider = services.BuildServiceProvider();
           //var db= provider.GetService<IDbProvider>();
           // using (var conn= db.CreateConnection())
           // {
           //     conn.Open();
           //     var cmd = conn.CreateCommand();
           //     cmd.CommandText = db.GetCreate<TestEntity>();
           //     cmd.ExecuteNonQuery();
           // }
            return provider;
        }
    }
}
