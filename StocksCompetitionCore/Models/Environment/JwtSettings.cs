using JetBrains.Annotations;

namespace StocksCompetitionCore.Models.Environment;

/// <summary>
/// Environment variables used for issuing and validating jwt tokens 
/// </summary>
[UsedImplicitly]
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
    /// Key string used for validating the tokens signature
    /// </summary>
    public required string Key { get; init; }
}