using Leo.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Leo.Logging.File
{
    public class FileLogger : BaseLogger
    {
        private readonly string categoryName;
        private readonly WorkQueue<string> workQueue;
    
        public FileLogger(string dir, string categoryName, IConfiguration configuration = null)
            : base(categoryName, configuration)
        {
            this.categoryName = categoryName;
            workQueue = new WorkQueue<string>(100, (s, e) =>
            {
                string message = string.Empty;
                message = string.Join(System.Environment.NewLine, e.Item);
                string fileFullName = Path.Combine(dir, $"{DateTime.Now.ToString("yyyyMMdd")}.log");
                using (FileStream output = new FileStream(fileFullName, FileMode.Append, FileAccess.Write, FileShare.Write))
                {
                    using (StreamWriter writer = new StreamWriter(output, Encoding.Unicode))
                    {
                        writer.WriteLine(message);
                    }
                }
            });
        }

        protected override string ProviderName => FileLoggerProvider.ProviderName;

        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;
            var message = GetMessage(logLevel, eventId, state, exception, formatter);
            message = $"{logLevel.ToString().Substring(0, 4)} {DateTime.Now.ToString("HH:mm:ss fff")} {message}";
            ThreadPool.QueueUserWorkItem(o => { workQueue.EnqueueItem(message); });
        }





    }
}
