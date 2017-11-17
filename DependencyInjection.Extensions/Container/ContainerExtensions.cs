using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Extensions.Container
{
    public static class ContainerExtensions
    {
        public static void AddDecorator<TService, TDecorator>(this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TService : class
            where TDecorator : class
        {
            var serviceDescriptor = new ServiceDescriptor(
                typeof(TService),
                provider => Construct<TService, TDecorator>(provider, services), lifetime);
            services.Add(serviceDescriptor);
        }

        private static TDecorator Construct<TService, TDecorator>(IServiceProvider serviceProvider,
            IServiceCollection services)
            where TDecorator : class
            where TService : class
        {
            var type = GetDecoratedType<TService>(services);
            var decoratedConstructor = GetConstructor(type);
            var decoratorConstructor = GetConstructor(typeof(TDecorator));
            var docoratedDependencies = serviceProvider.ResolveConstructorDependencies(
                decoratedConstructor.GetParameters());
            var decoratedService = decoratedConstructor.Invoke(docoratedDependencies.ToArray()) 
                as TService;
            var decoratorDependencies = serviceProvider.ResolveConstructorDependencies(
                decoratedService, 
                decoratorConstructor.GetParameters());
            return decoratorConstructor.Invoke(decoratorDependencies.ToArray()) as TDecorator;
        }

        private static Type GetDecoratedType<TService>(IServiceCollection services)
        {
            if (services.Count(p => 
                p.ServiceType == typeof(TService) && 
                p.ImplementationFactory == null) > 1)
            {
                throw new InvalidOperationException(
                    $"Only one decorated service for interface {nameof(TService)} allowed");
            }

            var nonFactoryDescriptor = services.FirstOrDefault(p => 
                p.ServiceType == typeof(TService) && 
                p.ImplementationFactory == null);
            return nonFactoryDescriptor?.ImplementationType;
        }

        private static ConstructorInfo GetConstructor(Type type)
        {
            var availableConstructors = type
                .GetConstructors()
                .Where(c => c.IsPublic)
                .ToList();
            
            if (availableConstructors.Count != 1)
            {
                throw new InvalidOperationException("Only single constructor types are supported");
            }
            return availableConstructors.First();
        }
    }
}