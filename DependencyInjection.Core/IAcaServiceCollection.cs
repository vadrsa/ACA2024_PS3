namespace DependencyInjection.Core;

public interface IAcaServiceCollection
{
    IAcaServiceCollection Add(Type serviceType, Type implementationType, ServiceLifetime lifetime);

    IAcaServiceProvider Build();
}