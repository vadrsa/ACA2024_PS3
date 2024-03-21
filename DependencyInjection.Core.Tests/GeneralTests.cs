namespace DependencyInjection.Core.Tests;

public class GeneralTests
{
    [Fact]
    public void AddServices_Build_Works()
    {
        var collection = new AcaServiceCollection();

        collection.Add(typeof(SomeService), typeof(SomeService), ServiceLifetime.Transient);
        collection.Add(typeof(SomeServiceL2), typeof(SomeServiceL2), ServiceLifetime.Singleton);

        var provider = collection.Build();

        provider.Should().NotBeNull();
    }
    
    
    [Fact]
    public void AddServicesOnlyRoot_GetBack_Throws()
    {
        var collection = new AcaServiceCollection();

        collection.Add(typeof(SomeService), typeof(SomeService), ServiceLifetime.Transient);

        var provider = collection.Build();
        
        var getService = () => provider.GetService(typeof(SomeService));
        getService.Should().Throw<Exception>();
    }
    
    [Fact]
    public void AddServices_GetBackUnregistered_ReturnsNull()
    {
        var collection = new AcaServiceCollection();

        collection.Add(typeof(SomeService), typeof(SomeService), ServiceLifetime.Transient);

        var provider = collection.Build();
        
        var service = provider.GetService(typeof(SomeServiceL2));
        service.Should().BeNull();
    }
    
    [Fact]
    public void AddServicesAll_GetBack_Works()
    {
        var collection = new AcaServiceCollection();

        collection.Add(typeof(SomeService), typeof(SomeService), ServiceLifetime.Transient);
        collection.Add(typeof(SomeServiceL2), typeof(SomeServiceL2), ServiceLifetime.Transient);
        collection.Add(typeof(SomeServiceL3), typeof(SomeServiceL3), ServiceLifetime.Transient);

        var provider = collection.Build();
        
        var service = provider.GetService(typeof(SomeService));
        service.Should().NotBeNull();
    }
    
    [Fact]
    public void AddServicesAll_GetBack_TransientWorks()
    {
        var collection = new AcaServiceCollection();

        collection.Add(typeof(SomeService), typeof(SomeService), ServiceLifetime.Transient);
        collection.Add(typeof(SomeServiceL2), typeof(SomeServiceL2), ServiceLifetime.Transient);
        collection.Add(typeof(SomeServiceL3), typeof(SomeServiceL3), ServiceLifetime.Transient);

        var provider = collection.Build();
        
        var service1 = provider.GetService(typeof(SomeService));
        var service2 = provider.GetService(typeof(SomeService));
        ReferenceEquals(service1, service2).Should().BeFalse();
    }
    
    [Fact]
    public void AddServicesAll_GetBack_SingletonWorks()
    {
        var collection = new AcaServiceCollection();

        collection.Add(typeof(SomeService), typeof(SomeService), ServiceLifetime.Singleton);
        collection.Add(typeof(SomeServiceL2), typeof(SomeServiceL2), ServiceLifetime.Transient);
        collection.Add(typeof(SomeServiceL3), typeof(SomeServiceL3), ServiceLifetime.Transient);

        var provider = collection.Build();
        
        var service1 = provider.GetService(typeof(SomeService));
        var service2 = provider.GetService(typeof(SomeService));
        ReferenceEquals(service1, service2).Should().BeTrue();
    }
}