using static InfrastructureUnitTests.TestUtilities.GetConfiguration;
using static InfrastructureUnitTests.TestUtilities.GetDependencies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using StocksCompetitionCore.Entities;
using StocksCompetitionCore.Repositories;
using SCAuthenticationService = StocksCompetitionInfrastructure.Services.AuthenticationService;

namespace InfrastructureUnitTests.Services.AuthenticationService;

public class InvalidateTests
{
    private readonly UserManager<ApplicationUser> _userManager = MockUserManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
    private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
    
    [Fact]
    public async Task Invalidate_Success()
    {
        // Arrange
        const string accessToken = "Access Token";
        var family = Guid.NewGuid();

        _refreshTokenRepository.GetByToken(Arg.Any<string>()).Returns(Task.FromResult<RefreshToken?>(GetRefreshToken(true, family)));
        
        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);
        
        // Act
        await authenticationService.Invalidate(accessToken, "Refresh Token");

        // Assert
        Assert.True(_memoryCache.TryGetValue($"logged-out-{accessToken}", out string? cachedToken));
        Assert.Equal(string.Empty , cachedToken);
        Received.InOrder(() => _refreshTokenRepository.Received().InvalidateFamily(Arg.Is<Guid>(received => received == family)));
    }

    [Fact]
    public async Task Invalidate_RefreshTokenNotFound()
    {
        // Arrange
        _refreshTokenRepository.GetByToken(Arg.Any<string>()).Returns(Task.FromResult<RefreshToken?>(null));
        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);

        // Act
        var result = await authenticationService.Invalidate("Access Token", "Refresh Token");

        // Assert
        Assert.False(result.Success);
        Assert.Equal(401, result.ErrorCode);
        Assert.Equal("Refresh token could not be found", result.ErrorMessage);
    }
}