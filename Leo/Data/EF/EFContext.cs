using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Leo.Data.EF
{

    public class EFContext : DbContext
    {
        private readonly IEntityTypeProvider modelProvider;

        public EFContext(DbContextOptions options, IEntityTypeProvider modelProvider) : base(options)
        {
            this.modelProvider = modelProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var types = modelProvider.GetTypes();
            foreach (var type in types)
            {
                modelBuilder.Model.AddEntityType(type);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
