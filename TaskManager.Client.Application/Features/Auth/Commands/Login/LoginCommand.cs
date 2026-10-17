using MediatR;
using TaskManager.Client.Application.DTOs;

namespace TaskManager.Client.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<LoginResponseDto>;