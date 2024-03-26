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

        public ServiceProvider(List<ServiceDescriptor> serviceDescriptors)
        {
            _serviceDescriptors = new Dictionary<Type, ServiceDescriptor>();

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
                    if (descriptor.Instance == null)
                    {
                        descriptor.Instance = CreateInstance(descriptor.ImplementationType);
                    }
                    return descriptor.Instance;
                }
                else if (descriptor.Lifetime == ServiceLifetime.Transient)
                {
                    return CreateInstance(descriptor.ImplementationType);
                }
            }
            return null;
        }

        private object CreateInstance(Type implementationType)
        {
            return Activator.CreateInstance(implementationType);
        }
    }
}