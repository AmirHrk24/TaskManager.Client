using MediatR;
using TaskManager.Client.Application.Abstractions;
using TaskManager.Client.Application.DTOs;

namespace TaskManager.Client.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IAuthApiClient authApiClient,
    ITokenProvider tokenProvider)
    : IRequestHandler<LoginCommand, LoginResponseDto>
{
    public async Task<LoginResponseDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var loginRequest = new LoginRequestDto
        {
            Email = request.Email,
            Password = request.Password
        };

        var response = await authApiClient.LoginAsync(
            loginRequest,
            cancellationToken);

        tokenProvider.SetToken(response.Token);

        return response;
    }
}