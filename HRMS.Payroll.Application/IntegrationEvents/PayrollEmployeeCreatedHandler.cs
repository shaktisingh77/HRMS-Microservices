using HRMS.Contracts.Employee;
using HRMS.Contracts.IntegrationEvents;
using HRMS.Payroll.Domain.Entities;
using HRMS.Payroll.Domain.Repositories;

namespace HRMS.Payroll.Application.IntegrationEvents;

public sealed class PayrollEmployeeCreatedHandler : IIntegrationEventHandler<EmployeeCreatedIntegrationEvent>
{
    private readonly IEmployeePayrollRepository _repository;

    public PayrollEmployeeCreatedHandler(IEmployeePayrollRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(EmployeeCreatedIntegrationEvent integrationEvent)
    {
        var employeePayroll = new EmployeePayroll(integrationEvent.EmployeeId);

        await _repository.AddAsync(employeePayroll);
    }
}