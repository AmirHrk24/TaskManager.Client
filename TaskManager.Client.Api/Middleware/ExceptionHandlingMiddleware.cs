using Microsoft.AspNetCore.Mvc;
using TaskManager.Client.Infrastructure.Exceptions;

namespace TaskManager.Client.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (TaskApiException ex)
        {
            logger.LogWarning(ex, "Task API request failed.");

            context.Response.StatusCode =
                ex.StatusCode ?? StatusCodes.Status502BadGateway;

            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "Task API request failed",
                Detail = ex.Message
            });
        }
    }
}