using MediatR;
using HRMS.Employee.Application.Commands.CreateEmployee;
using HRMS.Employee.Application.Commands.ResignEmployee;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Employee.API.Controllers;

[ApiController]
[Route("api/employees")]
public sealed class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateEmployeeCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("resign")]
    public async Task<IActionResult> Resign(
        ResignEmployeeCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}