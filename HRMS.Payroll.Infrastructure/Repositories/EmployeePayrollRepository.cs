using HRMS.Payroll.Domain.Entities;
using HRMS.Payroll.Domain.Repositories;
using HRMS.Payroll.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Payroll.Infrastructure.Repositories;

public class EmployeePayrollRepository : IEmployeePayrollRepository
{
    private readonly PayrollDbContext _dbContext;

    public EmployeePayrollRepository(PayrollDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EmployeePayroll?> GetByEmployeeIdAsync(Guid employeeId)
    {
        return await _dbContext.EmployeePayrolls
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
    }

    public async Task AddAsync(EmployeePayroll employeePayroll)
    {
        await _dbContext.EmployeePayrolls.AddAsync(employeePayroll);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(EmployeePayroll employeePayroll)
    {
        _dbContext.EmployeePayrolls.Update(employeePayroll);
        await _dbContext.SaveChangesAsync();
    }
}