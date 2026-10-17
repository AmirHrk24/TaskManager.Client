using System.Net.Http.Headers;
using System.Net.Http.Json;
using TaskManager.Client.Application.Abstractions;
using TaskManager.Client.Application.DTOs;
using TaskManager.Client.Infrastructure.Exceptions;

namespace TaskManager.Client.Infrastructure.HttpClients;

public sealed class TaskApiClient(
    HttpClient httpClient,
    ITokenProvider tokenProvider)
    : ITaskApiClient
{
    public async Task<PagedTasksDto> GetTasksAsync(
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        using var request = CreateRequest(
            HttpMethod.Get,
            $"api/tasks?page={page}&pageSize={pageSize}");

        using var response = await httpClient.SendAsync(
            request,
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
        using var request = CreateRequest(
            HttpMethod.Get,
            $"api/tasks/{id}");

        using var response = await httpClient.SendAsync(
            request,
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

    private HttpRequestMessage CreateRequest(
        HttpMethod method,
        string requestUri)
    {
        var request = new HttpRequestMessage(method, requestUri);

        if (!string.IsNullOrWhiteSpace(tokenProvider.AccessToken))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    tokenProvider.AccessToken);
        }

        return request;
    }
}