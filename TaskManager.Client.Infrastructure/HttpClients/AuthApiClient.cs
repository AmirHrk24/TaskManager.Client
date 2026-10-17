using System.Net.Http.Json;
using TaskManager.Client.Application.Abstractions;
using TaskManager.Client.Application.DTOs;
using TaskManager.Client.Infrastructure.Exceptions;

namespace TaskManager.Client.Infrastructure.HttpClients;

public sealed class AuthApiClient(HttpClient httpClient)
    : IAuthApiClient
{
    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "api/auth/login",
            request,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new TaskApiException(
                $"Login failed with status code {(int)response.StatusCode}.",
                (int)response.StatusCode);
        }

        var result =
            await response.Content.ReadFromJsonAsync<LoginResponseDto>(
                cancellationToken);

        return result ?? throw new TaskApiException(
            "Login API returned an empty response.");
    }
}