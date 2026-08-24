using MediatR;

namespace HRMS.Employee.Application.Commands.CreateEmployee;

public sealed record CreateEmployeeCommand(Guid EmployeeId,string Name,string Email,
                                           Guid DepartmentId,DateTime JoiningDate) : IRequest;