using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Config
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注入appsettings.json到<see cref="IConfiguration"/>。
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            if (System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json"))
            {
                var builder = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // <== compile failing here
               .AddJsonFile("appsettings.json");
                var config = builder.Build();
                services.AddSingleton<IConfiguration>(config);
            }
          
            return services;
        }
    }
}
