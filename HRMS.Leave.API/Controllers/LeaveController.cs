using HRMS.Leave.Application.Queries.GetLeaveAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Leave.API.Controllers;

[ApiController]
[Route("api/leave")]
public sealed class LeaveController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{employeeId:guid}")]
    public async Task<IActionResult> GetLeaveAccount(Guid employeeId)
    {
        var query = new GetLeaveAccountQuery(employeeId);

        var result = await _mediator.Send(query);

        return Ok(result);
    }
}