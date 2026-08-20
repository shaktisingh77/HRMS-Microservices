using HRMS.Employee.Domain.DomainEvents;

namespace HRMS.Employee.Application.Abstractions;
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent);
}