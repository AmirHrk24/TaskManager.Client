using System.Net.Http.Json;
using TaskManager.Client.Application.Abstractions;
using TaskManager.Client.Application.DTOs;
using TaskManager.Client.Infrastructure.Exceptions;

namespace TaskManager.Client.Infrastructure.HttpClients;

public sealed class TaskApiClient(HttpClient httpClient)
    : ITaskApiClient
{
    public async Task<PagedTasksDto> GetTasksAsync(
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync(
            $"api/tasks?page={page}&pageSize={pageSize}",
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new TaskApiException(
                $"Task API request failed with status code {(int)response.StatusCode}.",
                (int)response.StatusCode);
        }

        var result = await response.Content.ReadFromJsonAsync<PagedTasksDto>(
            cancellationToken);

        return result ?? new PagedTasksDto();
    }

    public async Task<TaskDto> GetTaskByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync(
            $"api/tasks/{id}",
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new TaskApiException(
                $"Task API request failed with status code {(int)response.StatusCode}.",
                (int)response.StatusCode);
        }

        var result = await response.Content.ReadFromJsonAsync<TaskDto>(
            cancellationToken);

        return result ?? throw new TaskApiException(
            "Task API returned an empty response.");
    }
}