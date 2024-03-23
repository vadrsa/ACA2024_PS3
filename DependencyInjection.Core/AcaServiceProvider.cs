using DependencyInjection.Core;
using System;
using System.Collections.Generic;

public class AcaServiceProvider : IAcaServiceProvider
{
    private readonly Dictionary<Type, (Type implementationType, ServiceLifetime lifetime)> _services;
    private readonly Dictionary<Type, object> _singletonInstances;

    public AcaServiceProvider(Dictionary<Type, (Type, ServiceLifetime)> services)
    {
        _services = services;
        _singletonInstances = new Dictionary<Type, object>();
    }

    public object? GetService(Type serviceType)
    {
        if (!_services.TryGetValue(serviceType, out var serviceData))
            return null;

        if (serviceData.lifetime == ServiceLifetime.Singleton)
        {
            if (!_singletonInstances.TryGetValue(serviceType, out var instance))
            {
                instance = CreateServiceInstance(serviceData.implementationType);
                _singletonInstances[serviceType] = instance;
            }
            return instance;
        }
        else if (serviceData.lifetime == ServiceLifetime.Transient)
        {
            return CreateServiceInstance(serviceData.implementationType);
        }
        else
        {
            throw new InvalidOperationException($"Unsupported service lifetime: {serviceData.lifetime}");
        }
    }

    private object? CreateServiceInstance(Type implementationType)
    {
        var constructors = implementationType.GetConstructors();
        if (constructors.Length == 0)
        {
            throw new InvalidOperationException($"Cannot create instance of type '{implementationType.Name}'. No public constructors found.");
        }

        var constructor = constructors.FirstOrDefault(ctor =>
        {
            var parameters = ctor.GetParameters();
            return parameters.Length == 0 || parameters.All(p => _services.ContainsKey(p.ParameterType));
        });

        if (constructor == null)
        {
            throw new InvalidOperationException($"Cannot find a matching constructor for type '{implementationType.Name}'.");
        }

        var parameterInstances = constructor.GetParameters()
                                            .Select(p => GetService(p.ParameterType))
                                            .ToArray();

        return constructor.Invoke(parameterInstances);
    }

}
