using HRMS.Leave.Application.DTOs;
using MediatR;

namespace HRMS.Leave.Application.Queries.GetLeaveAccount;

public sealed record GetLeaveAccountQuery(Guid EmployeeId) : IRequest<LeaveAccountDto>;