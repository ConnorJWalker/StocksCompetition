using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace StocksCompetition.Infrastructure.Middleware;

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
        // Only run checks on endpoints requiring authorization
        if (httpContext.GetEndpoint()?.Metadata.GetMetadata<IAuthorizeData>() is not null)
        {
            if (TokenIsLoggedOut(httpContext) || TokenIsLoggedOutAll(httpContext))
            {
                httpContext.Response.StatusCode = 401;
                return;
            }
        }

        await _next(httpContext);
    }

    /// <summary>
    /// Checks cache to see if the access token being used has been previously logged out
    /// </summary>
    /// <param name="httpContext">Http Context object containing the access token</param>
    /// <returns>True if access token is logged out</returns>
    private bool TokenIsLoggedOut(HttpContext httpContext)
    {
        // Remove "Bearer" from the authorization header
        var accessToken = httpContext.Request.Headers.Authorization.ToString().Split(" ")[1];
        return _memoryCache.TryGetValue($"logged-out-{accessToken}", out _);
    }

    /// <summary>
    /// Checks cache to see if the access token being used was issued prior to the user
    /// logging out every token
    /// </summary>
    /// <param name="httpContext">Http Context object containing the jwt claims</param>
    /// <returns>True if access token was issued prior to the owning user's log out all request</returns>
    private bool TokenIsLoggedOutAll(HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirstValue("Id");
            
        // User has logged out all access tokens since the given access token was issues, prevent authentication
        if (string.IsNullOrWhiteSpace(userId) || !_memoryCache.TryGetValue($"logged-out-all-{userId}", out var loggedOutAtObject))
        {
            return false;
        }
        
        var issuedAtString = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Iat);
        if (issuedAtString is null || loggedOutAtObject is null)
        {
            return false;
        }
        
        var couldParseIat = long.TryParse(issuedAtString, out var issuedAt);
        var couldParseLoggedOutAt = long.TryParse(loggedOutAtObject.ToString(), out var loggedOutAt);

        return couldParseIat && couldParseLoggedOutAt && loggedOutAt > issuedAt;
    }
}