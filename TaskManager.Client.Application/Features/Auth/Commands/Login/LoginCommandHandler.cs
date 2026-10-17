using MediatR;
using TaskManager.Client.Application.Abstractions;
using TaskManager.Client.Application.DTOs;

namespace TaskManager.Client.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IAuthApiClient authApiClient)
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

        return await authApiClient.LoginAsync(
            loginRequest,
            cancellationToken);
    }
}