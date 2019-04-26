using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Data.EF
{
   public class EFContext:DbContext
    {
        public EFContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TestModel> testModels { get; set; }
    }
}
