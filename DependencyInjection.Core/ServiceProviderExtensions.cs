namespace DependencyInjection.Core;

public static class ServiceProviderExtensions
{
    public static TService? GetService<TService>(this IAcaServiceProvider serviceProvider)
    where TService : class
    {
        return serviceProvider.GetService(typeof(TService)) as TService;
    }

    public static TService GetRequiredService<TService>(this IAcaServiceProvider serviceProvider)
    where TService : class
    {
        var service = serviceProvider.GetService(typeof(TService)) as TService;
        if(service == null)
        {
            throw new Exception($"Service of type '{typeof(TService).FullName}' is not registered.");
        }
        return service;
    }
}