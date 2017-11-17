using System;
using System.Collections.Generic;
using System.Reflection;

namespace DependencyInjection.Extensions.Container
{
    public static class ServiceProviderExtensions
    {
        public static List<object> ResolveConstructorDependencies<TService>(
            this IServiceProvider serviceProvider,
            TService decorated,
            IEnumerable<ParameterInfo> constructorParameters)
        {
            var depencenciesList = new List<object>();
            foreach (var parameter in constructorParameters)
            {
                if (parameter.ParameterType == typeof(TService))
                {
                    depencenciesList.Add(decorated);
                }
                else
                {
                    var resolvedDependency = serviceProvider.GetService(parameter.ParameterType);
                    depencenciesList.Add(resolvedDependency);
                }
            }
            return depencenciesList;
        }

        public static List<object> ResolveConstructorDependencies(
            this IServiceProvider serviceProvider,
            IEnumerable<ParameterInfo> constructorParameters)
        {
            var depencenciesList = new List<object>();
            foreach (var parameter in constructorParameters)
            {
                var resolvedDependency = serviceProvider.GetService(parameter.ParameterType);
                depencenciesList.Add(resolvedDependency);
            }
            return depencenciesList;
        }
    }
}