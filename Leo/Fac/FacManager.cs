using Leo.Data;
using Leo.Data.Dapper;
using Leo.Data.EF;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Leo.Fac
{
    public class FacManager
    {
        private static readonly IServiceCollection services;
        static FacManager()
        {
            services = new ServiceCollection();
            services.AddScoped<IUnitOfWork, DapperUnitOfWork>();
            services.AddSingleton<IDbProvider>(new SqlClientDbProvider(""));
            services.AddSingleton<ISqlBulider, SqlBulider>();
           // services.AddDbContext<EFContext>(options => options.UseInMemoryDatabase("data"));
            services.AddSingleton<IRepositoryBuilder, DapperRepositoryBuilder>();
        }

        public static T GetServcie<T>()
        {
          return  services.BuildServiceProvider().GetService<T>();
        }

    }
}
