namespace DependencyInjection.Core;

public static class ServiceCollectionExtensions{
    public static IAcaServiceCollection AddTransient<TService, TImplementation>(this IAcaServiceCollection services)
    {
        services.Add(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
        return services;
    }

    public static IAcaServiceCollection AddTransient<TService>(this IAcaServiceCollection services)
    {
        services.Add(typeof(TService), typeof(TService), ServiceLifetime.Transient);
        return services;
    }

    public static IAcaServiceCollection AddSingleton<TService, TImplementation>(this IAcaServiceCollection services)
    {
        services.Add(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
        return services;
    }

    public static IAcaServiceCollection AddSingleton<TService>(this IAcaServiceCollection services)
    {
        services.Add(typeof(TService), typeof(TService), ServiceLifetime.Singleton);
        return services;
    }
}