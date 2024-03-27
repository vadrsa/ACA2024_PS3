namespace DependencyInjection.Core;

public class AcaServiceCollection : IAcaServiceCollection
{
    private readonly List<ServiceDescriptor> _serviceDescriptors = new List<ServiceDescriptor>();
    public IAcaServiceCollection Add(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        _serviceDescriptors.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
        return this;
    }

    public IAcaServiceProvider Build()
    {
        return new ServiceProvider(_serviceDescriptors);
    }

    private class ServiceProvider : IAcaServiceProvider
    {
        private readonly Dictionary<Type, ServiceDescriptor> _serviceDescriptors;
        private readonly Dictionary<Type, object> _serviceInstances;

        public ServiceProvider(List<ServiceDescriptor> serviceDescriptors)
        {
            _serviceDescriptors = new Dictionary<Type, ServiceDescriptor>();
            _serviceInstances = new Dictionary<Type, object>();

            foreach (var descriptor in serviceDescriptors)
            {
                _serviceDescriptors[descriptor.ServiceType] = descriptor;
            }
        }

        public object? GetService(Type serviceType)
        {
            if (_serviceDescriptors.TryGetValue(serviceType, out var descriptor))
            {
                if (descriptor.Lifetime == ServiceLifetime.Singleton)
                {
                    if (!_serviceInstances.TryGetValue(serviceType, out var instance))
                    {
                        instance = CreateInstance(descriptor.ImplementationType);
                        _serviceInstances[serviceType] = instance;
                    }
                    return instance;
                }
                else if (descriptor.Lifetime == ServiceLifetime.Transient)
                {
                    return CreateInstance(descriptor.ImplementationType);
                }
            }
            return null;
        }

        private object? CreateInstance(Type implementationType)
        {
            var constructor = implementationType.GetConstructors()[0];
            var parameters = constructor.GetParameters();
            var parameterInstances = new List<object>();

            foreach (var parameter in parameters)
            {
                var parameterInstance = GetService(parameter.ParameterType);
                if (parameterInstance == null)
                {
                    throw new InvalidOperationException($"Cannot resolve service for type '{parameter.ParameterType}'.");
                }
                parameterInstances.Add(parameterInstance);
            }

            return Activator.CreateInstance(implementationType, parameterInstances.ToArray());
        }
    }
}