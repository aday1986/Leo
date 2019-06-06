using Leo.Data.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Data
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services,IDbProvider  dbProvider)
        {
            services.AddSingleton<IDbProvider>(dbProvider);
            services.AddTransient(typeof(Query<>));
            services.AddScoped<IDML, DML>();
            return services;
        }
    }
}
