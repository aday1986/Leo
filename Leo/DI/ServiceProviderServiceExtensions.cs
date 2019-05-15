using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.DI
{
  public static partial class ServiceProviderServiceExtensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            return Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetService<T>(provider);
        }

        public static IEnumerable<T> GetServices<T>(this IServiceProvider provider)
        {
            return Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetServices<T>(provider);
        }
    }
}
