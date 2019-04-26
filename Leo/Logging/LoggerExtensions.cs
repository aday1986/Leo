using Microsoft.Extensions.Logging;
using System;

namespace Leo.Logging
{
    public static partial class LoggerExtensions
    {
        public static void Log(this ILogger logger, LogInfo logInfo)
        {
            if (Enum.TryParse<LogLevel>(logInfo.LogLevel, out LogLevel logLevel))
            {
                logger.Log(logLevel, new EventId(logInfo.EventId, logInfo.EventName), logInfo.Message);
            }
        }
    }
}
