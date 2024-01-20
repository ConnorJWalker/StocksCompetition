using Microsoft.EntityFrameworkCore;
using StocksCompetition.Core.Entities;
using StocksCompetition.Core.Repositories;

namespace StocksCompetition.Infrastructure.Repositories;

/// <inheritdoc />
internal class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly StocksCompetitionDbContext _databaseContext;
    
    public RefreshTokenRepository(StocksCompetitionDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    /// <inheritdoc />
    public async Task<RefreshToken?> GetByToken(string token)
    {
        return await _databaseContext.RefreshTokens.FirstOrDefaultAsync(refresh => refresh.Token == token);
    }
    
    /// <inheritdoc />
    public async Task Create(string token, int userId, Guid family)
    {
        await _databaseContext.RefreshTokens.AddAsync(new RefreshToken
        {
            UserId = userId,
            Token = token,
            Family = family,
            Valid = true
        });

        await _databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task InvalidateToken(int id)
    {
        await _databaseContext.RefreshTokens
            .Where(refresh => refresh.Id == id)
            .ExecuteUpdateAsync(properties => properties.SetProperty(refresh => refresh.Valid, false));
    }

    /// <inheritdoc />
    public async Task InvalidateFamily(Guid family)
    {
        await _databaseContext.RefreshTokens
            .Where(refresh => refresh.Family == family)
            .ExecuteUpdateAsync(properties => properties.SetProperty(refresh => refresh.Valid, false));
    }

    public async Task InvalidateAllTokensForUser(int userId)
    {
        await _databaseContext.RefreshTokens
            .Where(refresh => refresh.UserId == userId)
            .ExecuteUpdateAsync(properties => properties.SetProperty(refresh => refresh.Valid, false));
    }
}