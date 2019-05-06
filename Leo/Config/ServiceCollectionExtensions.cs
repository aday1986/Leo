using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Config
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // <== compile failing here
                 .AddJsonFile("appsettings.json");
            var config = builder.Build();
            services.AddSingleton<IConfiguration>(config);
            return services;
        }
    }
}
