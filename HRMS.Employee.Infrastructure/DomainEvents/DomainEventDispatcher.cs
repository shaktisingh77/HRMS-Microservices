using HRMS.Employee.Application.DomainEvents;
using HRMS.Employee.Domain.DomainEvents;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Employee.Infrastructure.DomainEvents;

public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent)
    {
        var handlerType = typeof(IDomainEventHandler<>)
                                .MakeGenericType(domainEvent.GetType());

        var handlers = _serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            if (handler is null)
                continue;

            await ((dynamic)handler).HandleAsync((dynamic)domainEvent);
        }
    }
}