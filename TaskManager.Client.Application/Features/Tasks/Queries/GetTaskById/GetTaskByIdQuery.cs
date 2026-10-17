using MediatR;
using TaskManager.Client.Application.DTOs;

namespace TaskManager.Client.Application.Features.Tasks.Queries.GetTaskById;

public sealed record GetTaskByIdQuery(int Id) : IRequest<TaskDto>;