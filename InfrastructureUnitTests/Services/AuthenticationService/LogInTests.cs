using System.IdentityModel.Tokens.Jwt;
using static InfrastructureUnitTests.TestUtilities.GetConfiguration;
using static InfrastructureUnitTests.TestUtilities.GetDependencies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using StocksCompetitionCore.Entities;
using StocksCompetitionCore.Models.DataTransferObjects.Requests.Authentication;
using StocksCompetitionCore.Repositories;
using SCAuthenticationService = StocksCompetitionInfrastructure.Services.AuthenticationService;

namespace InfrastructureUnitTests.Services.AuthenticationService;

public class LogInTests
{
    private readonly UserManager<ApplicationUser> _userManager = MockUserManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
    private readonly IMemoryCache _memoryCache = Substitute.For<IMemoryCache>();
    
    private const string ErrorMessage = "Email or Password is incorrect";
    
    [Fact]
    public async Task LogIn_Success()
    {
        // Arrange
        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult<ApplicationUser?>(TestUser));
        _userManager.CheckPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(Task.FromResult(true));
        
        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);
        var logInRequest = new LogInRequest { Email = "test@test.com", Password = "TestPassword" };
        
        // Act
        var result = await authenticationService.LogIn(logInRequest);

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task LogIn_SuccessContainsCorrectClaims()
    {
        // Arrange
        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult<ApplicationUser?>(TestUser));
        _userManager.CheckPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(Task.FromResult(true));
        
        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);
        var logInRequest = new LogInRequest { Email = "test@test.com", Password = "TestPassword" };

        var tokenHandler = new JwtSecurityTokenHandler();
        
        // Act
        var result = await authenticationService.LogIn(logInRequest);
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
    public async Task LogIn_EmailNotFound()
    {
        // Arrange
        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult<ApplicationUser?>(null));
        
        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);
        var logInRequest = new LogInRequest { Email = "notfound@test.com", Password = "TestPassword" };

        // Act
        var result = await authenticationService.LogIn(logInRequest);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(401, result.ErrorCode);
        Assert.Equal(ErrorMessage, result.ErrorMessage);
    }

    [Fact]
    public async Task LogIn_PasswordIncorrect()
    {
        // Arrange
        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult<ApplicationUser?>(null));
        _userManager.CheckPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(Task.FromResult(false));
        
        var authenticationService = new SCAuthenticationService(_userManager, EnvironmentSettings, _refreshTokenRepository, _memoryCache);
        var logInRequest = new LogInRequest { Email = "test@test.com", Password = "IncorrectPassword" };
        
        // Act
        var result = await authenticationService.LogIn(logInRequest);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(401, result.ErrorCode);
        Assert.Equal(ErrorMessage, result.ErrorMessage);
    }
}