using MediatR;
using TaskManager.Client.Application.Abstractions;
using TaskManager.Client.Application.DTOs;

namespace TaskManager.Client.Application.Features.Tasks.Queries.GetTaskById;

public sealed class GetTaskByIdQueryHandler(
    ITaskApiClient taskApiClient)
    : IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    public async Task<TaskDto> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await taskApiClient.GetTaskByIdAsync(
            request.Id,
            cancellationToken);
    }
}