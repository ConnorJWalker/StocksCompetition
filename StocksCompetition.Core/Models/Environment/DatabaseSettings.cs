using JetBrains.Annotations;

namespace StocksCompetition.Core.Models.Environment;

/// <summary>
/// Environment variables used for interacting with the database
/// </summary>
public record DatabaseSettings
{
    /// <summary>
    /// The url that the database is hosted on
    /// </summary>
    public required string Host { get; init; }

    /// <summary>
    /// The username to use to log into the database
    /// </summary>
    public required string User { get; init; }
    
    /// <summary>
    /// The password to se to log into the database
    /// </summary>
    public required string Password { get; init; }
    
    /// <summary>
    /// The name of the database used in this project
    /// </summary>
    public required string Database { get; init; }
}