using HRMS.Payroll.Application.DTOs;
using HRMS.Payroll.Domain.Repositories;
using MediatR;

namespace HRMS.Payroll.Application.Queries.GetEmployeePayroll;

public sealed class GetEmployeePayrollQueryHandler : IRequestHandler<GetEmployeePayrollQuery, EmployeePayrollDto?>
{
    private readonly IEmployeePayrollRepository _repository;

    public GetEmployeePayrollQueryHandler(IEmployeePayrollRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmployeePayrollDto?> Handle(GetEmployeePayrollQuery query,CancellationToken cancellationToken)
    {
        var employeePayroll = await _repository.GetByEmployeeIdAsync(query.EmployeeId);

        if (employeePayroll is null)
            return null;

        return new EmployeePayrollDto(
            employeePayroll.EmployeeId,
            employeePayroll.IsActive);
    }
}