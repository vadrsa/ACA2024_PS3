namespace DependencyInjection.Core;

public interface IAcaServiceProvider
{
    object? GetService(Type serviceType);
}