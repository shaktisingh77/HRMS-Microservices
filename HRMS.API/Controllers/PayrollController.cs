using HRMS.Payroll.Application.Queries.GetEmployeePayroll;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/payroll")]
public class PayrollController : ControllerBase
{
    private readonly GetEmployeePayrollQueryHandler _handler;

    public PayrollController(GetEmployeePayrollQueryHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("{employeeId:guid}")]
    public async Task<IActionResult> GetEmployeePayroll(Guid employeeId)
    {
        var result = await _handler.HandleAsync(new GetEmployeePayrollQuery(employeeId));

        if (result is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            employeeId,
            isActive = result
        });
    }
}