using DependencyInjection.Core;
using DependencyInjection.Core.Extensions;
using System.Collections.Generic;


AcaServiceCollection serviceCollection = new AcaServiceCollection();
serviceCollection.Add(typeof(IService1), typeof(ServiceA), ServiceLifetime.Singleton);
serviceCollection.Add(typeof(IService1), typeof(ServiceC), ServiceLifetime.Singleton);
serviceCollection.Add(typeof(IService2), typeof(ServiceB), ServiceLifetime.Transient);
var serviceProvider = serviceCollection.Build();

var service1 = (IService1)serviceProvider.GetService(typeof(IService1));
var service3 = (IService1)serviceProvider.GetService(typeof(IService1));
service1.Execute();
service3.Execute();
Console.WriteLine(service1.GetType().Name);
Console.WriteLine(service3.GetType().Name);

var service2 = (IService2)serviceProvider.GetService(typeof(IService2));
service2.Execute();
Console.WriteLine(service2.GetType().Name);



public interface IService1
{
    public void Execute();
}
public interface IService2
{
    public void Execute();
}
public class ServiceA : IService1
{
    public void Execute()
    {
        Console.WriteLine("Service A executed");
    }
}

public class ServiceC : IService1
{
    public void Execute()
    {
        Console.WriteLine("Service C executed");
    }
}

public class ServiceB : IService2
{
    public void Execute()
    {
        Console.WriteLine("Service B executed");
    }
}


