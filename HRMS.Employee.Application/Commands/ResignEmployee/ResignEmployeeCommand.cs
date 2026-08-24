using MediatR;

namespace HRMS.Employee.Application.Commands.ResignEmployee
{
    public sealed record ResignEmployeeCommand(Guid EmployeeId,DateTime ResignationDate) : IRequest;
}
