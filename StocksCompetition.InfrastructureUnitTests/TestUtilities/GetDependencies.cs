using Microsoft.AspNetCore.Identity;
using StocksCompetition.Core.Entities;

namespace StocksCompetition.InfrastructureUnitTests.TestUtilities;

internal static class GetDependencies
{
    public static UserManager<ApplicationUser> MockUserManager
    {
        get
        {
            var userStore = Substitute.For<IUserStore<ApplicationUser>>();
            return Substitute.For<UserManager<ApplicationUser>>(userStore, null, null, null, null, null, null, null, null);
        }
    }
    
    public static readonly ApplicationUser TestUser = new ApplicationUser
    {
        Id = 1,
        DisplayName = "Test User",
        UserName = "TestUser",
        ProfilePicture = "Profile Picture",
        DisplayColour = "#d94eb8",
        IsAdmin = false
    };
    
    public static RefreshToken GetRefreshToken(bool valid, Guid family) => new RefreshToken
    {
        Id = 1,
        UserId = 1,
        Token = "Token",
        Family = family,
        Valid = valid
    };
}