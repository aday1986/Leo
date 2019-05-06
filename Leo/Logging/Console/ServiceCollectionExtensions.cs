using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;


namespace Leo.Logging.Console
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsole(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            services.AddLogging(builder => builder.AddProvider(new ConsoleLoggerProvider(configuration)));
            return services;
        }
    }
}
