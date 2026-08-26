using HRMS.Contracts.Employee;
using HRMS.Contracts.IntegrationEvents;
using HRMS.Employee.Domain.Repositories;
using MediatR;

namespace HRMS.Employee.Application.Commands.ResignEmployee;

public sealed class ResignEmployeeCommandHandler : IRequestHandler<ResignEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IIntegrationEventDispatcher _integrationEventDispatcher;

    public ResignEmployeeCommandHandler(IEmployeeRepository employeeRepository,
                                        IIntegrationEventDispatcher integrationEventDispatcher)
    {
        _employeeRepository = employeeRepository;
        _integrationEventDispatcher = integrationEventDispatcher;
    }

    public async Task Handle(ResignEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId);

        if (employee is null)
        {
            throw new InvalidOperationException("Employee not found.");
        }

        employee.Resign(command.ResignationDate);

        await _employeeRepository.UpdateAsync(employee);

        await _integrationEventDispatcher.DispatchAsync(
                                          new EmployeeResignedIntegrationEvent(
                                          employee.EmployeeId,
                                          employee.ResignationDate!.Value));
    }
}