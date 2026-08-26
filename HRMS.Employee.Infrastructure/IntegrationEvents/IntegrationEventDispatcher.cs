using HRMS.Contracts.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Employee.Infrastructure.IntegrationEvents;

public sealed class IntegrationEventDispatcher : IIntegrationEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public IntegrationEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IIntegrationEvent integrationEvent)
    {
        var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(integrationEvent.GetType());

        var handlers = _serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            if (handler is null)
                continue;

            await ((dynamic)handler).HandleAsync((dynamic)integrationEvent);
        }
    }
}