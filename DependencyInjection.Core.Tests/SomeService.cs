namespace DependencyInjection.Core.Tests;

public class SomeService
{
    public SomeService()
    {

    }
    
    public SomeService(SomeServiceL2 someServiceL2)
    {
        
    }
}
public class SomeServiceL2
{
    public SomeServiceL2(SomeServiceL3 someServiceL3)
    {
        
    }
}
public class SomeServiceL3
{
}