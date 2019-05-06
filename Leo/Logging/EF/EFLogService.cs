using Leo.Fac;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leo.Logging.EF
{
    [Service(ServiceType = typeof(ILogService))]
   public class EFLogService: ILogService
    {
        private readonly LogContext db;

        public EFLogService(LogContext db)
        {
            this.db = db;
        }

        public async void AddAsync(LogInfo log)
        {
           await db.Set<LogInfo>().AddAsync(log);
           await db.SaveChangesAsync();
        }
    }
}
