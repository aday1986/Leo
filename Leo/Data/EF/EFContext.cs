using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace Leo.Data.EF
{

    public class EFContext : DbContext
    {
        private readonly IEntityTypeCollection types;

        static EFContext()
        {
            SQLitePCL.Batteries.Init();//初始化sqlite
        }

        public EFContext(DbContextOptions options, IEntityTypeCollection types) : base(options)
        {
            this.types = types;

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //动态加载实体模型。

            foreach (var type in types)
            {
                modelBuilder.Model.AddEntityType(type);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
