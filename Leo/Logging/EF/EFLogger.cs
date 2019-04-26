using Leo.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;

namespace Leo.Logging.EF
{
    public class EFLogger : ILogger
    {
        private readonly string categoryName;
        private readonly ILogService logService;
        private readonly LoggerFilterOptions options;
        private WorkQueue<LogInfo> workQueue = new WorkQueue<LogInfo>();

        public EFLogger(string categoryName, ILogService logService, LoggerFilterOptions options=null)
        {
            this.categoryName = categoryName;
            this.logService = logService;
            this.options = options;
            workQueue.UserWork += WorkQueue_UserWork;
        }

        private void WorkQueue_UserWork(object sender, WorkQueue<LogInfo>.EnqueueEventArgs e)
        {
                lock (logService)//这里必须要锁实际调用的logService。
                {
                    logService.AddAsync(e.Item);
                }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (options == null)
                return true;
            if (logLevel <= options.MinLevel)
                return false;
            var rule = options.Rules
                .Where(r => r.ProviderName == EFLoggerProvider.ProviderName && categoryName.StartsWith(r.CategoryName))
                .OrderByDescending(r => r.CategoryName.Length)
                .FirstOrDefault();
            if (rule==null)//如果是null则判断是否有默认rule
            {
                rule = options.Rules.Where(r => r.ProviderName == EFLoggerProvider.ProviderName && r.CategoryName=="Default").FirstOrDefault();
            }
            if (rule==null)
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
                    isEnabled = isEnabled && rule.Filter.Invoke(EFLoggerProvider.ProviderName, categoryName, logLevel);
                }
                return isEnabled;
            }
          
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;
            //获取日志信息
            var message = $"{categoryName}:" + formatter?.Invoke(state, exception);

            LogInfo log = new LogInfo()
            { 
                CreateTime = DateTime.Now,
                LogLevel = logLevel.ToString(),
                Message = message,
                EventId = eventId.Id,
                EventName = eventId.Name
            };
            ThreadPool.QueueUserWorkItem(o => { workQueue.EnqueueItem(log); });
        }
    }
}
