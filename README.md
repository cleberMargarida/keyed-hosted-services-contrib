# KeyedBackgroundService for .NET

A simple implementation of keyed background services for .NET, allowing multiple hosted services to be registered with different keys.

## Features

- Register multiple background services with unique keys
- Each service starts automatically with the host
- Maintains all functionality of the standard `BackgroundService`
- Works with .NET's keyed dependency injection

## Usage

### 1. Create your service implementation

```csharp
public class MyService(string key, ILogger<MyService> logger) : KeyedBackgroundService(key)
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Your service logic here
            await Task.Delay(1000, stoppingToken);
        }
    }
}
```

### 2. Register services

```csharp
builder.Services.AddKeyedHostedService<MyService>("service1");
builder.Services.AddKeyedHostedService<MyService>("service2");
```

## API

### IKeyedHostedService

Interface for implementing a short running as OG `IHostedService` but keyed.

### KeyedBackgroundService

Base class for implementing a long running as OG `BackgroundService` but keyed.

**Constructor:**
- `KeyedBackgroundService(TKey serviceKey, ILogger logger)`

**Properties:**
- `Key` - The key for this service instance

### Extension Methods

**AddKeyedBackgroundService<TService>(ServiceKey)**
- Registers a keyed background service


## Execution Order Considerations

The current implementation groups keyed background services together in execution, which may affect startup order when mixed with standard hosted services.

### Behavior Details

When registering a mix of standard and keyed hosted services:

1. Standard `IHostedService` registrations execute in registration order
2. Keyed background services execute as a group when the last keyed is registered
3. Within the keyed services group, execution order follows registration order

Example:
```csharp
builder.Services.AddKeyedHostedService<KeyedService>("service1");  // Third (first in keyed group)
builder.Services.AddHostedService<StandardService1>();             // First to execute
builder.Services.AddHostedService<StandardService2>();             // Second to execute
builder.Services.AddKeyedHostedService<KeyedService>("service2");  // Fourth (second in keyed group)
```
```csharp
builder.Services.AddKeyedHostedService<KeyedService>("service1");  // Second (first in keyed group)
builder.Services.AddHostedService<StandardService1>();             // First to execute
builder.Services.AddKeyedHostedService<KeyedService>("service2");  // Third (second in keyed group)
builder.Services.AddHostedService<StandardService2>();             // Fourth to execute
```

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/git/git-scm.com/blob/main/MIT-LICENSE.txt) file for details.

## Contact

For any questions or support, please reach out to cleber.margarida@outlook.com
