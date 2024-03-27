namespace DependencyInjection.Core;

public class AcaServiceCollection : IAcaServiceCollection
{
    private readonly Dictionary<Type, (Type implementationType, ServiceLifetime lifetime)> _services;

    public AcaServiceCollection()
    {
        _services = new Dictionary<Type, (Type, ServiceLifetime)>();
    }

    public IAcaServiceCollection Add(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        _services[serviceType] = (implementationType, lifetime);
        return this;
    }

    public IAcaServiceProvider Build()
    {
        return new AcaServiceProvider(_services);
    }

    private class AcaServiceProvider : IAcaServiceProvider
    {
        private readonly Dictionary<Type, (Type implementationType, ServiceLifetime lifetime)> _services;
        private readonly Dictionary<Type, object?> _instances;

        public AcaServiceProvider(Dictionary<Type, (Type, ServiceLifetime)> services)
        {
            _services = services;
            _instances = new Dictionary<Type, object?>();
        }

        public object? GetService(Type serviceType)
        {
            if (_services.TryGetValue(serviceType, out var serviceInfo))
            {
                if (serviceInfo.lifetime == ServiceLifetime.Singleton)
                {
                    if (_instances.TryGetValue(serviceType, out var instance) == false)
                    {
                        instance = CreateServiceInstance(serviceInfo.implementationType, this);
                        _instances[serviceType] = instance;
                    }

                    return instance;
                }
                else if (serviceInfo.lifetime == ServiceLifetime.Transient)
                {
                    return CreateServiceInstance(serviceInfo.implementationType, this);
                }
            }
            return null;
        }

        private static object CreateServiceInstance(Type serviceType, IAcaServiceProvider serviceProvider)
        {
            var constructors = serviceType.GetConstructors();
            if (constructors.Length == 0)
            {
                throw new InvalidOperationException($"No public constructors found for type {serviceType}.");
            }

            var constructor = constructors.OrderByDescending(c => c.GetParameters().Length).First();
            var parameters = constructor.GetParameters();

            var args = parameters.Select(p =>
            {
                var dependency = serviceProvider.GetService(p.ParameterType);
                if (dependency is null)
                {
                    throw new InvalidOperationException($"Failed to resolve dependency of type {p.ParameterType} for {serviceType}.");
                }
                return dependency;
            }).ToArray();

            return constructor.Invoke(args);
            
        }
    }
}

public static class ServiceCollectionExtensions
{
    public static IAcaServiceCollection AddTransient<TService, TImplementation>(this IAcaServiceCollection services)
    {
        return services.Add(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
    }

    public static IAcaServiceCollection AddTransient<TService>(this IAcaServiceCollection services)
    {
        return services.AddTransient<TService, TService>();
    }

    public static IAcaServiceCollection AddSingleton<TService, TImplementation>(this IAcaServiceCollection services)
    {
        return services.Add(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
    }

    public static IAcaServiceCollection AddSingleton<TService>(this IAcaServiceCollection services)
    {
        return services.AddSingleton<TService, TService>();
    }
}

public static class ServiceProviderExtensions
{
    private static TService? GetService<TService>(this IAcaServiceProvider serviceProvider)
    {
        return (TService?)serviceProvider.GetService(typeof(TService));
    }

    public static TService GetRequiredService<TService>(this IAcaServiceProvider serviceProvider)
    {
        var service = serviceProvider.GetService<TService>();
        if (service == null)
        {
            throw new Exception($"Service of type {typeof(TService)} is not registered.");
        }

        return service;
    }
}