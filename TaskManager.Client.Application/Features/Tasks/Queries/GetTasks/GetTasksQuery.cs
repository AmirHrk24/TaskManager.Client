using MediatR;
using TaskManager.Client.Application.DTOs;

namespace TaskManager.Client.Application.Features.Tasks.Queries.GetTasks;

public sealed record GetTasksQuery(
    int Page = 1,
    int PageSize = 20) : IRequest<PagedTasksDto>;