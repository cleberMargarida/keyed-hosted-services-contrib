namespace Microsoft.Extensions.Hosting;

/// <summary>
/// Base class for implementing a long running <see cref="IHostedService"/> with keyed registration support.
/// </summary>
public abstract class KeyedBackgroundService(object key) : BackgroundService, IKeyedHostedService
{
    protected object Key { get; } = key;
}
