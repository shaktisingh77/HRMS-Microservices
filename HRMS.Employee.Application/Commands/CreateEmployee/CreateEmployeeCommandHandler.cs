using MediatR;
using HRMS.Employee.Domain.Repositories;
using EmployeeEntity = HRMS.Employee.Domain.Entities.Employee;
using HRMS.Contracts.IntegrationEvents;
using HRMS.Contracts.Employee;

namespace HRMS.Employee.Application.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IIntegrationEventDispatcher _integrationEventDispatcher;

    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository,IIntegrationEventDispatcher integrationEventDispatcher)
    {
        _employeeRepository = employeeRepository;
        _integrationEventDispatcher = integrationEventDispatcher;
    }

    public async Task Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = new EmployeeEntity(
            command.EmployeeId,
            command.Name,
            command.Email,
            command.DepartmentId,
            command.JoiningDate);

        await _employeeRepository.AddAsync(employee);

        await _integrationEventDispatcher.DispatchAsync(new EmployeeCreatedIntegrationEvent(employee.EmployeeId));
    }
}