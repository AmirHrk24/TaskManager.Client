using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Client.Application.DTOs;
using TaskManager.Client.Application.Features.Tasks.Queries.GetTaskById;
using TaskManager.Client.Application.Features.Tasks.Queries.GetTasks;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.Client.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class TasksController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedTasksDto>> GetTasks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    
    {
        if (page < 1)
            return BadRequest("Page must be greater than 0.");

        if (pageSize < 1 || pageSize > 100)
            return BadRequest("PageSize must be between 1 and 100.");
        var result = await sender.Send(
            new GetTasksQuery(page, pageSize),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskDto>> GetTaskById(
    int id,
    CancellationToken cancellationToken)
    {
        if (id < 1)
            return BadRequest("Task ID must be greater than 0.");
        var result = await sender.Send(
            new GetTaskByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }
}