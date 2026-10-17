namespace TaskManager.Client.Application.Abstractions;

public interface ITokenProvider
{
    string? AccessToken { get; }
    void SetToken(string token);
    void ClearToken();
}