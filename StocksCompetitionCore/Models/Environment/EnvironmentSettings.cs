using JetBrains.Annotations;

namespace StocksCompetitionCore.Models.Environment;

/// <summary>
/// Object containing information about the running environment
/// </summary>
[UsedImplicitly]
public record EnvironmentSettings
{
    /// <summary>
    /// Values used in the issuing and validation of json web tokens received for authentication
    /// </summary>
    public required JwtSettings Jwt { get; init; }
    
    /// <summary>
    /// Values used by Entity Framework to interact with the database
    /// </summary>
    public required DatabaseSettings Database { get; init; }
}