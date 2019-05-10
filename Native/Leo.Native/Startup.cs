using Leo.Data;
using Leo.Data.Dapper;
using Leo.Logging.File;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using Unity.Microsoft.DependencyInjection;
using Leo.Config;
using Leo.Native.Commands;

namespace Leo.Native
{
   public class Startup
    {
        public static IUnityContainer ConfigServices(IUnityContainer container)
        {
            IServiceCollection services = new ServiceCollection();
            string dir = $"{AppDomain.CurrentDomain.BaseDirectory }Data";
            string path = $"DataSource={dir}/msg.db";
            services.AddDapperRepository(new SqliteDbProvider(path));
            services.AddSingleton<ICommandActionList>(new CommandActionList());
            services.AddScoped<ICommandService, CommandService>();
            IServiceProvider provider = container.BuildServiceProvider(services);
         
            foreach (var service in services)
            {
                container.RegisterFactory(service.ServiceType, service.ServiceType.FullName, (e) => provider.GetService(service.ServiceType));
            }
            
            return container;
        }
    }
}
