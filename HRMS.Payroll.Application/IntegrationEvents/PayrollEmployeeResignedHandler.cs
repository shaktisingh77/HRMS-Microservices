using HRMS.Contracts.Employee;
using HRMS.Contracts.IntegrationEvents;

namespace HRMS.Payroll.Application.IntegrationEvents;

public sealed class PayrollEmployeeResignedHandler : IIntegrationEventHandler<EmployeeResignedIntegrationEvent>
{
    public Task HandleAsync(EmployeeResignedIntegrationEvent integrationEvent)
    {
        Console.WriteLine($"PAYROLL: Employee {integrationEvent.EmployeeId} resigned. Payroll action required.");

        return Task.CompletedTask;
    }
}