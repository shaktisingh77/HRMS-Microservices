using HRMS.Payroll.Domain.Repositories;

namespace HRMS.Payroll.Application.Queries.GetEmployeePayroll;

public sealed class GetEmployeePayrollQueryHandler
{
    private readonly IEmployeePayrollRepository _repository;

    public GetEmployeePayrollQueryHandler(IEmployeePayrollRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool?> HandleAsync(GetEmployeePayrollQuery query)
    {
        var employeePayroll =
            await _repository.GetByEmployeeIdAsync(query.EmployeeId);

        return employeePayroll?.IsActive;
    }
}