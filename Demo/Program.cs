using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Leo.Account;
using Leo.Data;
using Leo.Data.Dapper;
using Leo.Fac;
using Leo.Logging.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Leo.Data.AutoMapper;
using Leo.ThirdParty.AutoMapper;
using System.Linq.Expressions;
using Leo.Util;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Leo.Config;
using Leo.Logging.Console;
using Leo.Data.EF;

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
                        stopwatch.Stop();
                        Console.WriteLine($"完成{maxN * maxM}，共用时{stopwatch.Elapsed.TotalSeconds}秒。");
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
            
            services.AddAssembly(Assembly.GetEntryAssembly());
            //services.AddEFRepository(new EntityTypeProvider(new[] { typeof(UserInfo) }) , option => option.UseSqlite("Filename=data.db"));
            services.AddEFRepository(new EntityTypeProvider(new[] { typeof(UserInfo) }), option => option.UseInMemoryDatabase("data"));
            //services.AddDapperRepository(new SqliteDbProvider($"Data Source={AppDomain.CurrentDomain.BaseDirectory}data.db"));
           
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.Add(new ChainedConfigurationSource());
            services.AddConfiguration();
            //services.AddEFLogging();
            services.AddConsole();
            var r = Assembly.GetEntryAssembly().GetAllAssemblies();
            var types = Assembly.GetEntryAssembly().GetAllDefinedTypes();

#if DEBUG
            services.AddLogging(builder => builder.AddDebug());
#endif
        }
    }

    public class M : Profile
    {

    }

    public class ApplicationConfiguration
    {
        #region 属性成员

        /// <summary>
        /// 文件上传路径
        /// </summary>
        public string FileUpPath { get; set; }
        /// <summary>
        /// 是否启用单用户登录
        /// </summary>
        public bool IsSingleLogin { get; set; }
        /// <summary>
        /// 允许上传的文件格式
        /// </summary>
        public string AttachExtension { get; set; }
        /// <summary>
        /// 图片上传最大值KB
        /// </summary>
        public int AttachImagesize { get; set; }
        #endregion
    }


    public class AppConfigurtaionServices
    {
        private readonly IOptions<ApplicationConfiguration> _appConfiguration;
        public AppConfigurtaionServices(IOptions<ApplicationConfiguration> appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public ApplicationConfiguration AppConfigurations
        {
            get
            {
                return _appConfiguration.Value;
            }
        }
    }

}
