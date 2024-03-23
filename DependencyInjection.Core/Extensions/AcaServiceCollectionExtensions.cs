using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjection.Core.Extensions
{
    public static class AcaServiceCollectionExtensions
    {
        public static IAcaServiceCollection AddTransient<TService, TImplementation>(this IAcaServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            return services.Add(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
        }

        public static IAcaServiceCollection AddTransient<TService>(this IAcaServiceCollection services)
           where TService : class

        {
            return services.AddTransient<TService, TService>();
        }
        public static IAcaServiceCollection AddSingleton<TService, TImplementation>(this IAcaServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            return services.Add(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
        }

        public static IAcaServiceCollection AddSingleton<TService>(this IAcaServiceCollection services)
            where TService : class
        {
            return services.AddSingleton<TService, TService>();
        }
    }
}
