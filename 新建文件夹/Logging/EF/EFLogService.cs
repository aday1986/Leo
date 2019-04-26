using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leo.Logging.EF
{
   public class EFLogService: ILogService
    {
        private readonly LogContext db;

        public EFLogService(LogContext db)
        {
            this.db = db;
        }

        public async void AddAsync(LogInfo log)
        {
           await db.AddAsync<LogInfo>(log);
           await db.SaveChangesAsync();
        }
    }
}
