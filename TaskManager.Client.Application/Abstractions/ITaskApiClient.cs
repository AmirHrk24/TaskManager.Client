using TaskManager.Client.Application.DTOs;

namespace TaskManager.Client.Application.Abstractions;

public interface ITaskApiClient
{
    Task<PagedTasksDto> GetTasksAsync(
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    Task<TaskDto> GetTaskByIdAsync(
        int id,
        CancellationToken cancellationToken = default);
}