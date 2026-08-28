using HRMS.Leave.Domain.Entities;

namespace HRMS.Leave.Domain.Repositories;

public interface ILeaveAccountRepository
{
    Task<LeaveAccount?> GetByEmployeeIdAsync(Guid employeeId);

    Task AddAsync(LeaveAccount leaveAccount);

    Task UpdateAsync(LeaveAccount leaveAccount);
}