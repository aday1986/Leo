using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Logging.Console
{
    public class ConsoleLogger : BaseLogger
    {
        private static Dictionary<LogLevel, ConsoleColor> colors = new Dictionary<LogLevel, ConsoleColor>();

        private readonly string categoryName;

        static ConsoleLogger()
        {
            colors.Add(LogLevel.None, ConsoleColor.White);
            colors.Add(LogLevel.Trace, ConsoleColor.Blue);
            colors.Add(LogLevel.Debug, ConsoleColor.Blue);
            colors.Add(LogLevel.Information, ConsoleColor.Blue);
            colors.Add(LogLevel.Warning, ConsoleColor.Yellow);
            colors.Add(LogLevel.Error, ConsoleColor.Red);
            colors.Add(LogLevel.Critical, ConsoleColor.DarkRed);
            
        }

        public ConsoleLogger(string categoryName, IConfiguration configuration = null)
            :base(categoryName,configuration)
        {
            this.categoryName = categoryName;
        }

        protected override string ProviderName =>ConsoleLoggerProvider.ProviderName;

        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;
            var message = GetMessage(logLevel, eventId, state, exception, formatter);
            System.Console.ForegroundColor = colors[logLevel];
            System.Console.Write($"{DateTime.Now.ToString("yy-MM-dd HH:mm:ss")} " );
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine(message);
        }

        
       
    }
}
