using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StocksCompetitionCore.Entities;

namespace StocksCompetitionInfrastructure;

internal class StocksCompetitionDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public StocksCompetitionDbContext(DbContextOptions<StocksCompetitionDbContext> options) : base(options) { }

    public required DbSet<RefreshToken> RefreshTokens { get; init; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>(entityBuilder =>
        {
            entityBuilder.Ignore(user => user.PhoneNumber);
            entityBuilder.Ignore(user => user.PhoneNumberConfirmed);
            entityBuilder.Ignore(user => user.TwoFactorEnabled);
        });
        
        base.OnModelCreating(builder);
    }
}