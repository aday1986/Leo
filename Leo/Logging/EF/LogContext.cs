using Leo.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

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
          

            //this.Database.Migrate();
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.Model.AddEntityType(typeof(LogInfo));
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }

        public DbSet<LogInfo> LogInfo { get; set; }
    }


}
