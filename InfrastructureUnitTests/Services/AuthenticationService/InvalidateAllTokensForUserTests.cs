using static InfrastructureUnitTests.TestUtilities.GetConfiguration;
using static InfrastructureUnitTests.TestUtilities.GetDependencies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using StocksCompetitionCore.Entities;
using StocksCompetitionCore.Repositories;
using SCAuthenticationService = StocksCompetitionInfrastructure.Services.AuthenticationService;

namespace InfrastructureUnitTests.Services.AuthenticationService;

public class InvalidateAllTokensForUserTests
{
    private readonly UserManager<ApplicationUser> _userManager = MockUserManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
    private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
    
    [Fact]
    public async Task InvalidateAllTokensForUser_Success()
    {
        // Arrange
        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);
        
        // Act
        await authenticationService.InvalidateAllTokensForUser(TestUser.Id);

        // Assert
        Assert.True(_memoryCache.TryGetValue($"logged-out-all-{TestUser.Id}", out var cachedTimestamp));
        Assert.NotNull(cachedTimestamp);
        Assert.True(long.TryParse(cachedTimestamp.ToString(), out _));
        Received.InOrder(() => _refreshTokenRepository.Received().InvalidateAllTokensForUser(Arg.Is<int>(received => received == TestUser.Id)));
    }
}