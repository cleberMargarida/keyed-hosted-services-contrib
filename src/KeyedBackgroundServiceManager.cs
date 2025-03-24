namespace Microsoft.Extensions.Hosting;

using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

internal class KeyedBackgroundServiceManager(IReadOnlyCollection<ServiceDescriptor> keyedBackgroundServicesDescriptors, IServiceProvider serviceProvider) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken) => Task.WhenAll(
        keyedBackgroundServicesDescriptors.Select(
            service => serviceProvider.GetRequiredKeyedService<IKeyedHostedService>(
                service.ServiceKey).StartAsync(cancellationToken)));

    public Task StopAsync(CancellationToken cancellationToken) => Task.WhenAll(
        keyedBackgroundServicesDescriptors.Select(
            service => serviceProvider.GetRequiredKeyedService<IKeyedHostedService>(
                service.ServiceKey).StopAsync(cancellationToken)));
}
