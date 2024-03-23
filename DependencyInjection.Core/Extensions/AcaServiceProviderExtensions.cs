using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjection.Core.Extensions
{
    public static class AcaServiceProviderExtensions
    {
        public static TService? GetService<TService>(this IAcaServiceProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            return (TService?)provider.GetService(typeof(TService));
        }

        public static TService GetRequiredService<TService>(this IAcaServiceProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            var service = provider.GetService<TService>();
            if (service == null)
            {
                throw new InvalidOperationException($"Service of type '{typeof(TService).FullName}' not found.");
            }

            return service;
        }
    }
}
