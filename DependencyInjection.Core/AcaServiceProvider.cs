using System.Reflection;

namespace DependencyInjection.Core;

public class AcaServiceProvider : IAcaServiceProvider
{
    private readonly Dictionary<Type, ServiceDescriptor> _serviceDescriptors;
    private readonly Dictionary<Type, object> _singletons = new();

    public AcaServiceProvider(Dictionary<Type, ServiceDescriptor> serviceDescriptors)
    {
        _serviceDescriptors = serviceDescriptors;
    }
    public object? GetService(Type serviceType)
    {
        return GetServiceInternal(serviceType, true);
    }

    private object? GetServiceInternal(Type serviceType, bool isRoot)
    {
        if (_serviceDescriptors.TryGetValue(serviceType, out var descriptor))
        {
            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                if (_singletons.TryGetValue(descriptor.ImplementationType, out var singleton))
                {
                    return singleton;
                }
                else
                {
                    var obj = CreateNewInstance(descriptor.ImplementationType);
                    _singletons.Add(descriptor.ImplementationType, obj);
                    return obj;
                }
            }
            else
            {
                return CreateNewInstance(descriptor.ImplementationType);
            }
        }
        else
        {
            if (isRoot)
            {
                return null;
            }
            else
            {
                throw new InvalidOperationException("Service not found.");
            }
        }
    }

    private object CreateNewInstance(Type implementationType)
    {
        var constructor = implementationType.GetConstructors().FirstOrDefault();
        if (constructor == null)
        {
            throw new InvalidOperationException("Can't find a constructor for the implementation type.");
        }
            
        var parameters = constructor.GetParameters();

        var parameterObjects = parameters.Select(p => GetServiceInternal(p.ParameterType, false)).ToArray();

        return constructor.Invoke(parameterObjects);
    }
}