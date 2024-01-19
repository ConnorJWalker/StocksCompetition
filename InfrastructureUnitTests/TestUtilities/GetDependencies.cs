using Microsoft.AspNetCore.Identity;
using StocksCompetitionCore.Entities;

namespace InfrastructureUnitTests.TestUtilities;

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
}