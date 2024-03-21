namespace DependencyInjection.Core;

public class AcaServiceCollection : IAcaServiceCollection
{
    public IAcaServiceCollection Add(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        throw new NotImplementedException();
    }

    public IAcaServiceProvider Build()
    {
        throw new NotImplementedException();
    }
}