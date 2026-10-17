using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Client.Application.DTOs;
using TaskManager.Client.Application.Features.Auth.Commands.Login;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.Client.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(ISender sender) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Email and password are required.");
        }

        var result = await sender.Send(
            new LoginCommand(request.Email, request.Password),
            cancellationToken);

        return Ok(result);
    }
}