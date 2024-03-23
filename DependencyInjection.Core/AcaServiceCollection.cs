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

}