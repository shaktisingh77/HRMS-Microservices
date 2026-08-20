using HRMS.Employee.Domain.DomainEvents;

namespace HRMS.Employee.Application.Abstractions
{
    public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent domainEvent);
    }  
}
