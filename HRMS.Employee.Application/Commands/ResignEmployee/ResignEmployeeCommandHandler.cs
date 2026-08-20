using HRMS.Employee.Domain.Repositories;

namespace HRMS.Employee.Application.Commands.ResignEmployee;

public sealed class ResignEmployeeCommandHandler
{
    private readonly IEmployeeRepository _employeeRepository;

    public ResignEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task HandleAsync(ResignEmployeeCommand command)
    {
        var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId);

        if (employee is null)
        {
            throw new InvalidOperationException("Employee not found.");
        }

        employee.Resign(command.ResignationDate);

        await _employeeRepository.SaveAsync(employee);
    }
}