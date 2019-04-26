﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Data.EF
{
   public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEFRepository(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null)
        {
            services.AddDbContext<EFContext>(optionsAction, ServiceLifetime.Scoped);
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            return services;
        }

       

    }
}
