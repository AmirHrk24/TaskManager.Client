using TaskManager.Client.Application.Abstractions;

namespace TaskManager.Client.Infrastructure.Authentication;

public sealed class TokenProvider : ITokenProvider
{
    public string? AccessToken { get; private set; }

    public void SetToken(string token)
    {
        AccessToken = token;
    }

    public void ClearToken()
    {
        AccessToken = null;
    }
}