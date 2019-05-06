using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Leo.Logging
{
    public abstract class BaseLogger : ILogger
    {
        private readonly string categoryName;
        private readonly LoggerFilterOptions options;

        public BaseLogger(string categoryName,IConfiguration configuration=null)
        {
            this.categoryName = categoryName;
            options = GetLoggerFilterOptions(configuration.GetSection($"Logging:{ProviderName}"));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        protected abstract string ProviderName { get; }

        protected string GetMessage<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            return $"{ categoryName }:" + formatter?.Invoke(state, exception);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (options == null)
                return true;
            if (logLevel <= options.MinLevel)
                return false;
            var rule = options.Rules
                .Where(r => r.ProviderName == ProviderName && categoryName.StartsWith(r.CategoryName))
                .OrderByDescending(r => r.CategoryName.Length)
                .FirstOrDefault();
            if (rule == null)//如果是null则判断是否有默认rule
            {
                rule = options.Rules.Where(r => r.ProviderName == ProviderName && r.CategoryName == "Default").FirstOrDefault();
            }
            if (rule == null)
            {
                return true;
            }
            else
            {
                bool isEnabled = true;
                if (rule.LogLevel.HasValue)
                {
                    isEnabled = logLevel >= rule.LogLevel;
                }
                if (rule.Filter != null)
                {
                    isEnabled = isEnabled && rule.Filter.Invoke(ProviderName, categoryName, logLevel);
                }
                return isEnabled;
            }
        }

        public abstract void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);

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
