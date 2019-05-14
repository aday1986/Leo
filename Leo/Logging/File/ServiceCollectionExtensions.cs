using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Leo.Logging.File
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// 默认路径\\data\\log
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFileLogging(this IServiceCollection services)
        {
           var dir = $"{ AppDomain.CurrentDomain.BaseDirectory}\\data\\log";
           return services.AddFileLogging(dir);
        }

        public static IServiceCollection AddFileLogging(this IServiceCollection services, string dir)
        {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            services.AddLogging(builder => builder.AddProvider(new FileLoggerProvider(dir, configuration)));
            return services;
        }
    }
}
