// See https://aka.ms/new-console-template for more information

using DependencyInjection.Core;
using DependencyInjection.Playground;

var serviceCollection = new AcaServiceCollection();

serviceCollection.Add(typeof(IService), typeof(Service1), ServiceLifetime.Transient);

var sp = serviceCollection.Build();

var service = sp.GetService(typeof(IService));


