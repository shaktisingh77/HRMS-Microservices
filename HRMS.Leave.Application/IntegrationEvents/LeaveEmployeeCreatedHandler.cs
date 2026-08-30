using HRMS.Contracts.Employee;
using HRMS.Contracts.IntegrationEvents;
using HRMS.Leave.Domain.Entities;
using HRMS.Leave.Domain.Repositories;

namespace HRMS.Leave.Application.IntegrationEvents;

public sealed class LeaveEmployeeCreatedHandler : IIntegrationEventHandler<EmployeeCreatedIntegrationEvent>
{
    private readonly ILeaveAccountRepository _repository;

    public LeaveEmployeeCreatedHandler(ILeaveAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(EmployeeCreatedIntegrationEvent integrationEvent)
    {
        var existingLeaveAccount = await _repository.GetByEmployeeIdAsync(integrationEvent.EmployeeId);

        if (existingLeaveAccount is not null)
            return;

        var leaveAccount = new LeaveAccount(integrationEvent.EmployeeId);
        await _repository.AddAsync(leaveAccount);
    }
}