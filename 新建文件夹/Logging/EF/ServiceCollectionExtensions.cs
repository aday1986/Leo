using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;


namespace Leo.Logging.EF
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEFLogging(this IServiceCollection services, IConfiguration configuration = null)
        {
          
            services.AddDbContext<LogContext>(options => options
                .UseSqlite($"Filename={AppDomain.CurrentDomain.BaseDirectory}Logging/log.db")
                .UseLoggerFactory(new LoggerFactory())
                , ServiceLifetime.Singleton)
                .AddSingleton<ILogService, EFLogService>();
            return services.AddLogging(bulder =>
            {
                LoggerFilterOptions filterOptions = null;
                if (configuration != null)
                {
                    filterOptions = GetLoggerFilterOptions(configuration.GetSection($"Logging:{EFLoggerProvider.ProviderName}"));
                    bulder.AddConfiguration(configuration.GetSection("Logging"));
                }
                bulder
             .ClearProviders()
              .AddProvider(new EFLoggerProvider(services.BuildServiceProvider().GetService<ILogService>(), filterOptions))
             .AddConsole();
            });
        }


        private static LoggerFilterOptions GetLoggerFilterOptions(IConfigurationSection configuration)
        {
            if (configuration == null)
                return null;
            LoggerFilterOptions options = new LoggerFilterOptions();
            if (Enum.TryParse<LogLevel>(configuration.GetSection("MinLevel").Value, out LogLevel minLevel))
            {
                options.MinLevel = minLevel;
            };
            foreach (var item in configuration.GetSection("LogLevel").GetChildren())
            {
                if (Enum.TryParse(item.Value, out LogLevel logLevel))
                {
                    options.Rules.Add(
                        new LoggerFilterRule(configuration.Key, item.Key, logLevel, null));
                }
            }
            return options;
        }
    }
}
