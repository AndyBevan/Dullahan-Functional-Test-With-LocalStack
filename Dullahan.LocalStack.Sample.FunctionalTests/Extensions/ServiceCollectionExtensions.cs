using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Dullahan.LocalStack.Sample.FunctionalTests.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RemoveAll<TService>(this IServiceCollection services)
    {
        var serviceDescriptors = services.Where(descriptor => descriptor.ServiceType == typeof(TService)).ToList();

        foreach (var serviceDescriptor in serviceDescriptors)
        {
            services.Remove(serviceDescriptor);
        }
    }
}
