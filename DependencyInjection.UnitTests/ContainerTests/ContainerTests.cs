using DependencyInjection.Extensions.Container;
using DependencyInjection.UnitTests.ContainerTests.Fakes;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DependencyInjection.UnitTests.ContainerTests
{
    [TestFixture]
    public class ContainerTests
    {
        
        [Test]
        public void GivenDecoratorAndDecoratedRegistered_WhenResolvingTheirInterface_ThenExpectDecorator()
        {
            var servicesCollection = new ServiceCollection();

            servicesCollection.AddTransient<IDataSource, DataSource>();
            servicesCollection.AddDecorator<IDataSource, DataSource2>();

            var provider = servicesCollection.BuildServiceProvider(new ServiceProviderOptions());
            var service = provider.GetService<IDataSource>();

            Assert.IsTrue(service.GetType() == typeof(DataSource2));
            Assert.IsTrue(service.GetData() == "DecoratedData");
        }
    }
}