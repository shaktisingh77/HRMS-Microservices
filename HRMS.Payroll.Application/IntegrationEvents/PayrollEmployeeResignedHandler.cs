using HRMS.Contracts.Employee;
using HRMS.Contracts.IntegrationEvents;
using HRMS.Payroll.Domain.Repositories;

namespace HRMS.Payroll.Application.IntegrationEvents;

public sealed class PayrollEmployeeResignedHandler : IIntegrationEventHandler<EmployeeResignedIntegrationEvent>
{
    private readonly IEmployeePayrollRepository _repository;

    public PayrollEmployeeResignedHandler(IEmployeePayrollRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(EmployeeResignedIntegrationEvent integrationEvent)
    {
        var employeePayroll = await _repository.GetByEmployeeIdAsync(integrationEvent.EmployeeId);

        if (employeePayroll is null)
        {
            throw new InvalidOperationException($"Payroll record not found for employee {integrationEvent.EmployeeId}.");
        }

        employeePayroll.Deactivate();

        await _repository.UpdateAsync(employeePayroll);
    }
}