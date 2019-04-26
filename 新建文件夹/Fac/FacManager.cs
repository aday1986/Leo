using Leo.Data;
using Leo.Data.Dapper;
using Leo.Data.EF;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Leo.Logging.EF;

namespace Leo.Fac
{
    public class FacManager
    {
        static FacManager()
        {
            Services = new ServiceCollection();
        
            Services.AddEFLogging(null);
            //Services.AddDapperRepository(new SqlClientDbProvider(""));
            Services.AddEFRepository(option => option.UseInMemoryDatabase("data"));
        }

        public static T GetServcie<T>()
        {
            return ServiceProvider.GetService<T>();
        }

        public static IServiceCollection Services { get; private set; }

        public static ServiceProvider ServiceProvider { get => Services.BuildServiceProvider(); }

    }

    public static partial class ServiceCollectionServiceExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection  services)
        {
            return services;
        }

       


    }
}
