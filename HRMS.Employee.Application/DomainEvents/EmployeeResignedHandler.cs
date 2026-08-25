using HRMS.Employee.Domain.DomainEvents;

namespace HRMS.Employee.Application.DomainEvents;

public sealed class EmployeeResignedHandler : IDomainEventHandler<EmployeeResigned>
{
    public Task HandleAsync(EmployeeResigned domainEvent)
    {
        Console.WriteLine(
            $"Employee {domainEvent.EmployeeId} resigned on {domainEvent.ResignationDate:yyyy-MM-dd}");

        return Task.CompletedTask;
    }
}