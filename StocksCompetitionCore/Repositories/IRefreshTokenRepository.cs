using StocksCompetitionCore.Entities;

namespace StocksCompetitionCore.Repositories;

/// <summary>
/// Abstraction around the Refresh Tokens table in the database 
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Fetches refresh token with matching user given token
    /// </summary>
    /// <param name="token">Token supplied by the user</param>
    /// <returns>Refresh token entity if found, null if not</returns>
    public Task<RefreshToken?> GetByToken(string token);

    /// <summary>
    /// Creates a new refresh token record
    /// </summary>
    /// <param name="token">The token body supplied to the user</param>
    /// <param name="userId">The id of the user the token has been generated for</param>
    /// <param name="family">The family of tokens the new refresh token belongs to</param>
    public Task Create(string token, int userId, Guid family);

    /// <summary>
    /// Marks refresh token with matching id as invalid
    /// </summary>
    /// <param name="id">Id of the refresh token to invalidate</param>
    public Task InvalidateToken(int id);
    
    /// <summary>
    /// Marks all stored refresh tokens with matching family as invalid
    /// </summary>
    /// <param name="family">Guid of the family to invalidate</param>
    public Task InvalidateFamily(Guid family);
}