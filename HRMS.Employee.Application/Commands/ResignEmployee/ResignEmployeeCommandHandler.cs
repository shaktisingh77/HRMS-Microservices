using HRMS.Contracts.Employee;
using HRMS.Employee.Application.Outbox;
using HRMS.Employee.Domain.Repositories;
using MediatR;

namespace HRMS.Employee.Application.Commands.ResignEmployee;

public sealed class ResignEmployeeCommandHandler : IRequestHandler<ResignEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IOutboxWriter _outboxWriter;

    public ResignEmployeeCommandHandler(IEmployeeRepository employeeRepository, IOutboxWriter outboxWriter)
    {
        _employeeRepository = employeeRepository;
        _outboxWriter = outboxWriter;
    }

    public async Task Handle(ResignEmployeeCommand command,CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId);

        if (employee is null)
        {
            throw new InvalidOperationException(
                "Employee not found.");
        }

        employee.Resign(command.ResignationDate);

        await _employeeRepository.UpdateAsync(employee);

        var integrationEvent = new EmployeeResignedIntegrationEvent(employee.EmployeeId,employee.ResignationDate!.Value);

        await _outboxWriter.AddAsync(integrationEvent);

        await _employeeRepository.SaveChangesAsync();
    }
}