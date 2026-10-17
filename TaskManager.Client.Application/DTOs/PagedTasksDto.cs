namespace TaskManager.Client.Application.DTOs;

public sealed class PagedTasksDto
{
    public List<TaskDto> Items { get; set; } = [];

    public int TotalCount { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }
}