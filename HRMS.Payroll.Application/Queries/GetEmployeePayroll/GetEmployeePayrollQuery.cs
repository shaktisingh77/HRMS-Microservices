using HRMS.Payroll.Application.DTOs;
using MediatR;

namespace HRMS.Payroll.Application.Queries.GetEmployeePayroll;

public sealed record GetEmployeePayrollQuery(Guid EmployeeId) : IRequest<EmployeePayrollDto?>;