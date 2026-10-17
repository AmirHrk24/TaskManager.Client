using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace TaskManager.Client.Infrastructure.HttpClients;

public sealed class ForwardAuthorizationHandler(
    IHttpContextAccessor httpContextAccessor)
    : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authorizationHeader =
            httpContextAccessor.HttpContext?
                .Request.Headers.Authorization.ToString();

        if (!string.IsNullOrWhiteSpace(authorizationHeader) &&
            AuthenticationHeaderValue.TryParse(
                authorizationHeader,
                out var authorization))
        {
            request.Headers.Authorization = authorization;
        }

        return base.SendAsync(request, cancellationToken);
    }
}