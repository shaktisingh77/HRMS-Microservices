using HRMS.Payroll.Application.Queries.GetEmployeePayroll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Payroll.API.Controllers;

[ApiController]
[Route("api/payroll")]
public sealed class PayrollController : ControllerBase
{
    private readonly IMediator _mediator;

    public PayrollController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{employeeId:guid}")]
    public async Task<IActionResult> GetEmployeePayroll(Guid employeeId)
    {
        var query = new GetEmployeePayrollQuery(employeeId);

        var result = await _mediator.Send(query);

        return Ok(result);
    }
}