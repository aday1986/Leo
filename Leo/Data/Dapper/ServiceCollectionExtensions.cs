using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Leo.Logging.Sqlite;

namespace Leo.Data.Dapper
{
   
   public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDapperRepository(this IServiceCollection services,IDbProvider dbProvider)
        {
            services.AddScoped<IUnitOfWork, DapperUnitOfWork>();
            services.AddSingleton<ISqlBulider, SqlBulider>();
            services.AddSingleton<IDbProvider>(dbProvider);
            services.AddScoped(typeof(IRepository<>),typeof(DapperRepository<>));
            return services;
        }
    }
}
