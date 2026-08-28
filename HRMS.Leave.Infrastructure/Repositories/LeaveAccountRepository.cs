using HRMS.Leave.Domain.Entities;
using HRMS.Leave.Domain.Repositories;
using HRMS.Leave.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Leave.Infrastructure.Repositories;

public class LeaveAccountRepository : ILeaveAccountRepository
{
    private readonly LeaveDbContext _dbContext;

    public LeaveAccountRepository(LeaveDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LeaveAccount?> GetByEmployeeIdAsync(Guid employeeId)
    {
        return await _dbContext.LeaveAccounts.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
    }

    public async Task AddAsync(LeaveAccount leaveAccount)
    {
        await _dbContext.LeaveAccounts.AddAsync(leaveAccount);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(LeaveAccount leaveAccount)
    {
        _dbContext.LeaveAccounts.Update(leaveAccount);
        await _dbContext.SaveChangesAsync();
    }
}