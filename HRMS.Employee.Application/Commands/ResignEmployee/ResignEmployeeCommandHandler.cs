using HRMS.Employee.Domain.Repositories;
using MediatR;

namespace HRMS.Employee.Application.Commands.ResignEmployee;

public sealed class ResignEmployeeCommandHandler : IRequestHandler<ResignEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;

    public ResignEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
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
    }
}