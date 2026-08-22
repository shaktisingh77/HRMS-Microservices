using HRMS.Employee.Domain.Repositories;
using EmployeeEntity = HRMS.Employee.Domain.Entities.Employee;

namespace HRMS.Employee.Infrastructure.Repositories;

public sealed class EmployeeRepository : IEmployeeRepository
{
    public Task<EmployeeEntity?> GetByIdAsync(Guid employeeId)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(EmployeeEntity employee)
    {
        throw new NotImplementedException();
    }
}