﻿using Leo.Data;
using Leo.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Leo.Logging.Sqlite
{
    public class SqliteLogger : BaseLogger
    {
        private readonly string categoryName;
        private readonly IRepository<LogInfo> repository;
        private WorkQueue<LogInfo> workQueue;

        protected override string ProviderName => SqliteLoggerProvider.ProviderName;

        public SqliteLogger(string categoryName, Leo.Data.IRepository<LogInfo> repository, IConfiguration configuration = null)
            : base(categoryName, configuration)
        {
            this.categoryName = categoryName;
            this.repository = repository;
            workQueue = new WorkQueue<LogInfo>(1000, (s, e) =>
            {
                lock (repository)//这里要锁实际调用的logService。
                {
                    repository.AddRange(e.Item);
                    repository.SaveChanges();
                }
            });
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