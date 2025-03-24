using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for adding keyed hosted services to the IServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add an keyed <see cref="IKeyedHostedService"/> registration for the given type.
    /// </summary>
    /// <typeparam name="TKeyedHostedService">An <see cref="IKeyedHostedService"/> to register.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
    /// <param name="serviceKey">The <see cref="ServiceDescriptor.ServiceKey"/> of the service.</param>
    /// <returns>The original <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddKeyedHostedService<TKeyedHostedService>(this IServiceCollection services, object serviceKey)
        where TKeyedHostedService : class, IKeyedHostedService
    {
        services.AddKeyedSingleton<IKeyedHostedService>(serviceKey,
            static (serviceProvider, key) => ActivatorUtilities.CreateInstance<TKeyedHostedService>(serviceProvider, key));

        return AddKeyedHostedServiceInternal(services);
    }

    /// <summary>
    /// Add an keyed <see cref="IKeyedHostedService"/> registration for the given type.
    /// </summary>
    /// <typeparam name="TKeyedHostedService">An <see cref="IKeyedHostedService"/> to register.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
    /// <param name="serviceKey">The <see cref="ServiceDescriptor.ServiceKey"/> of the service.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>The original <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddKeyedHostedService<TKeyedHostedService>(this IServiceCollection services, object serviceKey, Func<IServiceProvider, object, TKeyedHostedService> implementationFactory) 
        where TKeyedHostedService : class, IKeyedHostedService
    {
        services.AddKeyedSingleton<IKeyedHostedService>(serviceKey, implementationFactory);

        return AddKeyedHostedServiceInternal(services);
    }


    private static IServiceCollection AddKeyedHostedServiceInternal(IServiceCollection services) => services.AddHostedService(
        serviceProvider => ActivatorUtilities.CreateInstance<KeyedBackgroundServiceManager>(serviceProvider,
            services.Where(x => x.IsKeyedService && typeof(IKeyedHostedService).IsAssignableFrom(x.ServiceType)).ToList().AsReadOnly()));
}
