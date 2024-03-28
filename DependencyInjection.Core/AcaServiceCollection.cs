namespace DependencyInjection.Core;

public class AcaServiceCollection : IAcaServiceCollection
{
    public Dictionary<Type, ServiceDescriptor> _serviceDescriptors = new();

    public IAcaServiceCollection Add(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        _serviceDescriptors[serviceType] = new ServiceDescriptor(serviceType, implementationType, lifetime);
        return this;
    }

    public IAcaServiceProvider Build()
    {
        return new AcaServiceProvider(_serviceDescriptors);
    }
}