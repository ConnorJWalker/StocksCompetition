using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace StocksCompetitionInfrastructure.Middleware;

/// <summary>
/// Middleware to ensure access tokens that have been logged out do not allow access to
/// authorized endpoints
/// </summary>
public class RejectLoggedOutUsersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _memoryCache;
    
    public RejectLoggedOutUsersMiddleware(RequestDelegate next, IMemoryCache memoryCache)
    {
        _next = next;
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// Checks the cache for the user's access token if the endpoint they are accessing requires
    /// authentication to ensure their token has not been logged out
    /// </summary>
    /// <param name="httpContext">Current http context</param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.GetEndpoint()?.Metadata.GetMetadata<IAuthorizeData>() is not null)
        {
            // Remove "Bearer" from the authorization header
            var accessToken = httpContext.Request.Headers.Authorization.ToString().Split(" ")[1];
            if (_memoryCache.TryGetValue($"logged-out-{accessToken}", out _))
            {
                httpContext.Response.StatusCode = 401;
                return;
            }
        }

        await _next(httpContext);
    }
}