using MediatR;
using HRMS.Employee.Domain.Repositories;
using EmployeeEntity = HRMS.Employee.Domain.Entities.Employee;

namespace HRMS.Employee.Application.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;

    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
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
    }
}