using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static InfrastructureUnitTests.TestUtilities.GetConfiguration;
using static InfrastructureUnitTests.TestUtilities.GetDependencies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using StocksCompetitionCore.Entities;
using StocksCompetitionCore.Repositories;
using SCAuthenticationService = StocksCompetitionInfrastructure.Services.AuthenticationService;

namespace InfrastructureUnitTests.Services.AuthenticationService;

public class RefreshTests
{
    private readonly UserManager<ApplicationUser> _userManager = MockUserManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
    private readonly IMemoryCache _memoryCache = Substitute.For<IMemoryCache>();
    
    private const string ErrorMessage = "Could not refresh token";
    
    [Fact]
    public async Task Refresh_Success()
    {
        // Arrange
        var token = GetRefreshTokenString(true);
        var family = Guid.NewGuid();
        _refreshTokenRepository.GetByToken(Arg.Any<string>()).Returns(Task.FromResult<RefreshToken?>(GetRefreshToken(true, family)));
        _userManager.FindByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<ApplicationUser?>(TestUser));
        
        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);
        
        // Act
        var result = await authenticationService.Refresh(token);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Content);
        Received.InOrder(() =>
        {
            _refreshTokenRepository.Received().InvalidateToken(Arg.Is<int>(received => received == 1));
            _refreshTokenRepository.Received().Create(
                Arg.Is<string>(received => received == result.Content.RefreshToken),
                Arg.Is<int>(received => received == TestUser.Id),
                Arg.Is<Guid>(received => received == family)
            );
        });
    }

    [Fact]
    public async Task Refresh_SuccessContainsCorrectClaims()
    {
        // Arrange
        var token = GetRefreshTokenString(true);
        var family = Guid.NewGuid();
        _refreshTokenRepository.GetByToken(Arg.Any<string>()).Returns(Task.FromResult<RefreshToken?>(GetRefreshToken(true, family)));
        _userManager.FindByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<ApplicationUser?>(TestUser));
        
        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);
        var tokenHandler = new JwtSecurityTokenHandler();
        
        // Act
        var result = await authenticationService.Refresh(token);
        var parsedAccessToken = tokenHandler.ReadJwtToken(result.Content?.AccessToken);
        var parsedRefreshToken = tokenHandler.ReadJwtToken(result.Content?.RefreshToken);
        
        // Assert
        Assert.NotNull(result.Content);
        Assert.NotNull(parsedAccessToken.Claims.FirstOrDefault(claim => claim.Type == "iat"));
        Assert.Equal(TestUser.Id.ToString(), parsedAccessToken.Claims.First(claim => claim.Type == "Id").Value);
        Assert.Equal(TestUser.DisplayName, parsedAccessToken.Claims.First(claim => claim.Type == "DisplayName").Value);
        Assert.Equal(TestUser.UserName, parsedAccessToken.Claims.First(claim => claim.Type == "UserName").Value);
        Assert.Equal(TestUser.ProfilePicture, parsedAccessToken.Claims.First(claim => claim.Type == "ProfilePicture").Value);
        Assert.Equal(TestUser.DisplayName, parsedAccessToken.Claims.First(claim => claim.Type == "DisplayName").Value);
        Assert.Equal(TestUser.IsAdmin.ToString(), parsedAccessToken.Claims.First(claim => claim.Type == "IsAdmin").Value);
        Assert.Equal(TestUser.Id.ToString(), parsedRefreshToken.Claims.First(claim => claim.Type == "Id").Value);
        
    }
    
    [Fact]
    public async Task Refresh_TokenNotFound()
    {
        // Arrange
        _refreshTokenRepository.GetByToken(Arg.Any<string>()).Returns(Task.FromResult<RefreshToken?>(null));
        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);
        
        // Act
        var result = await authenticationService.Refresh("NonExistentToken");

        // Assert
        Assert.False(result.Success);
        Assert.Equal(401, result.ErrorCode);
        Assert.Equal(ErrorMessage, result.ErrorMessage);
    }
    
    [Fact]
    public async Task Refresh_TokenInvalid()
    {
        // Arrange
        var family = Guid.NewGuid();
        _refreshTokenRepository.GetByToken(Arg.Any<string>()).Returns(Task.FromResult<RefreshToken?>(GetRefreshToken(false, family)));

        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);
        
        // Act
        var result = await authenticationService.Refresh("Token");
        
        // Assert
        Assert.False(result.Success);
        Assert.Equal(401, result.ErrorCode);
        Assert.Equal(ErrorMessage, result.ErrorMessage);
        Received.InOrder(() => _refreshTokenRepository.Received().InvalidateFamily(Arg.Is<Guid>(received => received == family)));
    }
    
    [Fact]
    public async Task Refresh_NoIdInToken()
    {
        // Arrange
        var token = GetRefreshTokenString(false);
        var family = Guid.NewGuid();
        
        _refreshTokenRepository.GetByToken(Arg.Any<string>()).Returns(Task.FromResult<RefreshToken?>(GetRefreshToken(true, family)));
        
        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);
        
        // Act
        var result = await authenticationService.Refresh(token);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(401, result.ErrorCode);
        Assert.Equal(ErrorMessage, result.ErrorMessage);
        Received.InOrder(() => _refreshTokenRepository.Received().InvalidateFamily(Arg.Is<Guid>(received => received == family)));
    }
    
    [Fact]
    public async Task Refresh_UserNotFound()
    {
        // Arrange
        var token = GetRefreshTokenString(true); 
        var family = Guid.NewGuid();
        
        _refreshTokenRepository.GetByToken(Arg.Any<string>()).Returns(Task.FromResult<RefreshToken?>(GetRefreshToken(true, family)));
        _userManager.FindByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<ApplicationUser?>(null));
        
        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);
        
        // Act
        var result = await authenticationService.Refresh(token);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(401, result.ErrorCode);
        Assert.Equal(ErrorMessage, result.ErrorMessage);
        Received.InOrder(() => _refreshTokenRepository.Received().InvalidateFamily(Arg.Is<Guid>(received => received == family)));
    }
    
    private static string GetRefreshTokenString(bool withId)
    {
        var token = new JwtSecurityToken(
            EnvironmentSettings.Jwt.Issuer,
            EnvironmentSettings.Jwt.Audience,
            withId ? [new Claim("Id", "1")] : [  /* No id claim */  ],
            DateTime.Now,
            DateTime.Now.AddDays(EnvironmentSettings.Jwt.RefreshTokenLifetimeDays),
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentSettings.Jwt.RefreshKey)), 
                SecurityAlgorithms.HmacSha256
            )
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}