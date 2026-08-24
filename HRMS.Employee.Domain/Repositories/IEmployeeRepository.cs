using EmployeeEntity = HRMS.Employee.Domain.Entities.Employee;

namespace HRMS.Employee.Domain.Repositories;

public interface IEmployeeRepository
{
    Task<EmployeeEntity?> GetByIdAsync(Guid employeeId);

    Task AddAsync(EmployeeEntity employee);

    Task UpdateAsync(EmployeeEntity employee);
}