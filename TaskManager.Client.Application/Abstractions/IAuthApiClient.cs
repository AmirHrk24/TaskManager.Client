using TaskManager.Client.Application.DTOs;

namespace TaskManager.Client.Application.Abstractions;

public interface IAuthApiClient
{
    Task<LoginResponseDto> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default);
}