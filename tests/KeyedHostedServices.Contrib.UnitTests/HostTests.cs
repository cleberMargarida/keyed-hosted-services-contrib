using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KeyedHostedServices.Contrib.Microsoft.Extensions.Hosting.UnitTests;

public class HostTests
{

    [Fact]
    public async Task WithValidRegistration_WhenServicesAreStartedAsync_ShouldCallKeys()
    {
        // Arrange
        var builder = Host.CreateEmptyApplicationBuilder(null);
        builder.Services.AddKeyedHostedService<Fixture>("key1");
        builder.Services.AddKeyedHostedService<Fixture>("key2");
        using IHost app = builder.Build();

        // Act
        await app.StartAsync();
        await app.StopAsync();

        // Assert
        Assert.Contains("key1", Fixture.KeysCalled);
        Assert.Contains("key2", Fixture.KeysCalled);
    }

    class Fixture(object key) : KeyedBackgroundService(key)
    {
        public static List<object> KeysCalled = new(2);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            KeysCalled.Add(Key);
            return Task.CompletedTask;
        }
    }
}