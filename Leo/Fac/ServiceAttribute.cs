using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Fac
{

    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute :Attribute
    {
        public string Name { get; set; }

        public Type ServiceType { get; set; }

        public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Scoped;
    }
}
