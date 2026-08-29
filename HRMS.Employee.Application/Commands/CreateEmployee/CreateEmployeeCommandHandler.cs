using HRMS.Contracts.Employee;
using HRMS.Employee.Application.Outbox;
using HRMS.Employee.Domain.Repositories;
using MediatR;
using EmployeeEntity = HRMS.Employee.Domain.Entities.Employee;

namespace HRMS.Employee.Application.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IOutboxWriter _outboxWriter;

    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IOutboxWriter outboxWriter)
    {
        _employeeRepository = employeeRepository;
        _outboxWriter = outboxWriter;
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

        var integrationEvent = new EmployeeCreatedIntegrationEvent(employee.EmployeeId);

        await _outboxWriter.AddAsync(integrationEvent);

        await _employeeRepository.SaveChangesAsync();
    }
}