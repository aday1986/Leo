using Microsoft.EntityFrameworkCore;

namespace Leo.Logging.EF
{
    public class LogContext : DbContext
    {
        static LogContext()
        {
            SQLitePCL.Batteries.Init();

        }
        public LogContext(DbContextOptions<LogContext> options) : base(options)
        {

        }

       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }

        public DbSet<LogInfo> LogInfo { get; set; }
    }


}
