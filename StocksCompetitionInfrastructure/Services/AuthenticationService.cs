using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StocksCompetitionCore.Entities;
using StocksCompetitionCore.Models;
using StocksCompetitionCore.Models.DataTransferObjects.Requests;
using StocksCompetitionCore.Models.DataTransferObjects.Responses;
using StocksCompetitionCore.Models.Environment;
using StocksCompetitionCore.Services;

namespace StocksCompetitionInfrastructure.Services;

/// <inheritdoc />
internal class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly EnvironmentSettings _environmentSettings;
    
    public AuthenticationService(UserManager<ApplicationUser> userManager, EnvironmentSettings environmentSettings)
    {
        _userManager = userManager;
        _environmentSettings = environmentSettings;
    }
    
    /// <inheritdoc />
    public async Task<Result<AuthenticationResponse>> SignUp(SignUpRequest signUpRequest)
    {
        if (await _userManager.FindByEmailAsync(signUpRequest.Email) is not null)
        {
            return Result<AuthenticationResponse>.FromFailed(400, $"User with email {signUpRequest.Email} already exists");
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
            ? Result<AuthenticationResponse>.FromSuccess(GenerateTokens(user))
            : Result<AuthenticationResponse>.FromFailed(500, "Could not create user account");
    }

    private AuthenticationResponse GenerateTokens(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new("DisplayName", user.DisplayName),
            new("UserName", user.UserName!),
            new("ProfilePicture", user.ProfilePicture),
            new("DisplayColour", user.DisplayColour),
            new("IsAdmin", user.IsAdmin.ToString())
        };

        var accessTokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_environmentSettings.Jwt.Key));
        var accessToken = new JwtSecurityToken(
            _environmentSettings.Jwt.Issuer,
            _environmentSettings.Jwt.Audience,
            claims,
            DateTime.Now,
            DateTime.Now.AddMinutes(10),
            new SigningCredentials(accessTokenKey, SecurityAlgorithms.HmacSha256)
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        
        return new AuthenticationResponse(tokenHandler.WriteToken(accessToken), "TODO: Refresh");
    }
}