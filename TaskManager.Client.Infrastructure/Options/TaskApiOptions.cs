namespace TaskManager.Client.Infrastructure.Options;

public sealed class TaskApiOptions
{
    public const string SectionName = "TaskApi";

    public string BaseUrl { get; set; } = string.Empty;
}