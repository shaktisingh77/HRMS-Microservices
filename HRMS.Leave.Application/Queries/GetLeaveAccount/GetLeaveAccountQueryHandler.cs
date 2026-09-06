using HRMS.Leave.Application.DTOs;
using HRMS.Leave.Domain.Repositories;
using MediatR;

namespace HRMS.Leave.Application.Queries.GetLeaveAccount;

public sealed class GetLeaveAccountQueryHandler : IRequestHandler<GetLeaveAccountQuery, LeaveAccountDto?>
{
    private readonly ILeaveAccountRepository _repository;

    public GetLeaveAccountQueryHandler(ILeaveAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<LeaveAccountDto?> Handle(GetLeaveAccountQuery request,CancellationToken cancellationToken)
    {
        var leaveAccount = await _repository.GetByEmployeeIdAsync(request.EmployeeId);

        if (leaveAccount is null)
            return null;

        return new LeaveAccountDto(leaveAccount.EmployeeId,leaveAccount.AvailableLeave);
    }
}