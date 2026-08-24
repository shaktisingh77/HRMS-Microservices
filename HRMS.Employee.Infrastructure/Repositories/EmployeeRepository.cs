using Microsoft.EntityFrameworkCore;
using HRMS.Employee.Domain.Repositories;
using EmployeeEntity = HRMS.Employee.Domain.Entities.Employee;
using HRMS.Employee.Infrastructure.Persistence;

namespace HRMS.Employee.Infrastructure.Repositories;

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDbContext _dbContext;

    public EmployeeRepository(EmployeeDbContext dbContext)
    {
        _dbContext = dbContext;
    }    

    public async Task<EmployeeEntity?> GetByIdAsync(Guid employeeId)
    {
        return await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
    }

    public async Task AddAsync(EmployeeEntity employee)
    {
        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(EmployeeEntity employee)
    {
        _dbContext.Employees.Update(employee);
        await _dbContext.SaveChangesAsync();
    }
}