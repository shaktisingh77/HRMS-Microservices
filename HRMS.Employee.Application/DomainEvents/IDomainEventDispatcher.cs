using HRMS.Employee.Domain.DomainEvents;

namespace HRMS.Employee.Application.DomainEvents;
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent);
}