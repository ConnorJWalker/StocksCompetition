using Microsoft.AspNetCore.Identity;
using StocksCompetition.Core.Entities;
using StocksCompetition.Core.Models;
using StocksCompetition.Core.Models.DataTransferObjects.Requests.Authentication;
using StocksCompetition.Core.Services;

namespace StocksCompetition.Infrastructure.Services;

/// <inheritdoc />
internal class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    /// <inheritdoc />
    public async Task<Result<bool>> Create(SignUpRequest signUpRequest)
    {
        if (await _userManager.FindByEmailAsync(signUpRequest.Email) is not null)
        {
            return Result<bool>.FromFailed(400, "An account already exists with this email");
        }
        
        var user = new ApplicationUser
        {
            DisplayName = signUpRequest.DisplayName,
            ProfilePicture = "TODO: profile pictures",
            DisplayColour = signUpRequest.DisplayColour,
            Email = signUpRequest.Email,
            UserName = signUpRequest.UserName
        };
        
        var userCreatedResult = await _userManager.CreateAsync(user, signUpRequest.Password);
        return userCreatedResult.Succeeded
            ? Result<bool>.FromSuccess(true)
            : Result<bool>.FromFailed(500, "An error occured attempting to create account");
    }

    /// <inheritdoc />
    public async Task<bool> GetEmailIsUnique(string email) => await _userManager.FindByEmailAsync(email) is null;
}