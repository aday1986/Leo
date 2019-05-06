using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using Leo.Util;

namespace Leo.Fac
{
    public static partial class ServiceCollectionServiceExtensions
    {

        public static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetAllDefinedTypes().Where(t => t.GetCustomAttribute<ServiceAttribute>() != null && t.IsPublic);
            foreach (var type in types)
            {
                var att = type.GetCustomAttribute<ServiceAttribute>();
                //注册自身。
                services.Add(new ServiceDescriptor(type, type, att.ServiceLifetime));
                //注册特性指定类型，没有则注册所有继承的非系统接口。
                if (att.ServiceType!=null )
                {
                    services.Add(new ServiceDescriptor(att.ServiceType, type, att.ServiceLifetime));
                }
                else
                {
                    var interfaces = type.GetInterfaces()
                        .Where (t=>!t.Namespace.StartsWith("System") && !t.Namespace.StartsWith("Microsoft"));
                    foreach (var @interface in interfaces)
                    {
                        services.Add(new ServiceDescriptor(@interface, type, att.ServiceLifetime));
                    }
                }
            }
            return services;
        }


    }
}
