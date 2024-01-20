using static InfrastructureUnitTests.TestUtilities.GetConfiguration;
using static InfrastructureUnitTests.TestUtilities.GetDependencies;
using Microsoft.AspNetCore.Identity;
using StocksCompetitionCore.Entities;
using StocksCompetitionCore.Models.DataTransferObjects.Requests.Authentication;
using SCUserService = StocksCompetitionInfrastructure.Services.UserService;

namespace InfrastructureUnitTests.Services.UserService;

public class CreateTests
{
    private readonly UserManager<ApplicationUser> _userManager = MockUserManager;

    private readonly SignUpRequest _signUpRequest = new SignUpRequest
    {
        Email = TestUser.Email!,
        UserName = TestUser.UserName!,
        DisplayName = TestUser.DisplayName,
        DisplayColour = TestUser.DisplayColour,
        Password = "test-password",
        PasswordConfirm = "test-password"
    };
    
    [Fact]
    public async Task Create_Success()
    {
        // Arrange
        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult<ApplicationUser?>(null));
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(Task.FromResult(IdentityResult.Success));
        
        var userService = new SCUserService(_userManager);
        
        // Act
        var result = await userService.Create(_signUpRequest);
        
        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task Create_EmailAlreadyExists()
    {
        // Arrange
        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult<ApplicationUser?>(TestUser));
        var userService = new SCUserService(_userManager);
        
        // Act
        var result = await userService.Create(_signUpRequest);
        
        // Assert
        Assert.False(result.Success);
        Assert.Equal(400, result.ErrorCode);
        Assert.Equal("An account already exists with this email", result.ErrorMessage);
    }
    
    [Fact]
    public async Task Create_UserManagerCreateFailed()
    {
        // Arrange
        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult<ApplicationUser?>(null));
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(Task.FromResult(IdentityResult.Failed()));
        
        var userService = new SCUserService(_userManager);
        
        // Act
        var result = await userService.Create(_signUpRequest);
        
        // Assert
        Assert.False(result.Success);
        Assert.Equal(500, result.ErrorCode);
        Assert.Equal("An error occured attempting to create account", result.ErrorMessage);
    }
}