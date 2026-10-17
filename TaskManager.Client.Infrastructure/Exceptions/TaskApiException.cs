namespace TaskManager.Client.Infrastructure.Exceptions;

public sealed class TaskApiException(
    string message,
    int? statusCode = null)
    : Exception(message)
{
    public int? StatusCode { get; } = statusCode;
}