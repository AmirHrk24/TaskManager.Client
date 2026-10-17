using MediatR;
using TaskManager.Client.Application.Abstractions;
using TaskManager.Client.Application.DTOs;

namespace TaskManager.Client.Application.Features.Tasks.Queries.GetTasks;

public sealed class GetTasksQueryHandler(
    ITaskApiClient taskApiClient)
    : IRequestHandler<GetTasksQuery, PagedTasksDto>
{
    public async Task<PagedTasksDto> Handle(
        GetTasksQuery request,
        CancellationToken cancellationToken)
    {
        return await taskApiClient.GetTasksAsync(
            request.Page,
            request.PageSize,
            cancellationToken);
    }
}