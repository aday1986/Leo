using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;


namespace Leo.Logging.File
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileLogging(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            services.AddLogging(builder => builder.AddProvider(new FileLoggerProvider(configuration)));
            return services;
        }
    }
}
