using Leo.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;

namespace Leo.Logging.EF
{
    public class EFLogger : LoggerBase
    {
        private readonly string categoryName;
        private readonly ILogService logService;
        private readonly LoggerFilterOptions options;
        private WorkQueue<LogInfo> workQueue = new WorkQueue<LogInfo>();

        protected override string ProviderName => EFLoggerProvider.ProviderName;

        public EFLogger(string categoryName, ILogService logService, LoggerFilterOptions options=null) 
            :base(categoryName,options)
        {
            this.categoryName = categoryName;
            this.logService = logService;
            this.options = options;
            workQueue.UserWork += WorkQueue_UserWork;
        }

        private void WorkQueue_UserWork(object sender, EnqueueEventArgs<LogInfo> e)
        {
                lock (logService)//这里要锁实际调用的logService。
                {
                    logService.AddAsync(e.Item);
                }
        }


        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = GetMessage(logLevel, eventId, state, exception, formatter);

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
