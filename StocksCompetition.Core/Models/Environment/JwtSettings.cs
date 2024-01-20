using JetBrains.Annotations;

namespace StocksCompetition.Core.Models.Environment;

/// <summary>
/// Environment variables used for issuing and validating jwt tokens 
/// </summary>
public record JwtSettings
{
    /// <summary>
    /// Url of server to allow tokens signed from 
    /// </summary>
    public required string Issuer { get; init; }

    /// <summary>
    /// Url of client to validate tokens from
    /// </summary>
    public required string Audience { get; init; }

    /// <summary>
    /// Key string used for validating the access tokens signature
    /// </summary>
    public required string Key { get; init; }
    
    /// <summary>
    /// Key string used for validating the refresh tokens signature
    /// </summary>
    public required string RefreshKey { get; init; }
    
    /// <summary>
    /// Duration that an access token is valid for before requiring a refresh in minutes
    /// </summary>
    public required int AccessTokenLifetimeMinutes { get; init; }
    
    /// <summary>
    /// Duration that a refresh token is valid for before requiring re-authentication in days
    /// </summary>
    public required int RefreshTokenLifetimeDays { get; init; }
}