using HRMS.Payroll.Domain.Entities;

namespace HRMS.Payroll.Domain.Repositories;

public interface IEmployeePayrollRepository
{
    Task<EmployeePayroll?> GetByEmployeeIdAsync(Guid employeeId);

    Task AddAsync(EmployeePayroll employeePayroll);

    Task UpdateAsync(EmployeePayroll employeePayroll);
}