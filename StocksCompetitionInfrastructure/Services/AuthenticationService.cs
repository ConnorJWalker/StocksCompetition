using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StocksCompetitionCore.Entities;
using StocksCompetitionCore.Models;
using StocksCompetitionCore.Models.DataTransferObjects.Requests.Authentication;
using StocksCompetitionCore.Models.DataTransferObjects.Responses;
using StocksCompetitionCore.Models.Environment;
using StocksCompetitionCore.Repositories;
using StocksCompetitionCore.Services;

namespace StocksCompetitionInfrastructure.Services;

/// <inheritdoc />
internal class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly EnvironmentSettings _environmentSettings;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    private const string LogInErrorMessage = "Email or Password is incorrect";
    private const string RefreshErrorMessage = "Could not refresh token";
    
    public AuthenticationService(UserManager<ApplicationUser> userManager, EnvironmentSettings environmentSettings,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _environmentSettings = environmentSettings;
        _refreshTokenRepository = refreshTokenRepository;
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

        if (!userCreatedResult.Succeeded)
        {
            return Result<AuthenticationResponse>.FromFailed(500, "Could not create user account");
        }

        var tokens = GenerateTokens(user);
        await _refreshTokenRepository.Create(tokens.RefreshToken, user.Id, Guid.NewGuid());

        return Result<AuthenticationResponse>.FromSuccess(tokens);
    }

    /// <inheritdoc />
    public async Task<Result<AuthenticationResponse>> LogIn(LogInRequest logInRequest)
    {
        var user = await _userManager.FindByEmailAsync(logInRequest.Email); 
        if (user is null)
        {
            return Result<AuthenticationResponse>.FromFailed(401, LogInErrorMessage);
        }

        if (!await _userManager.CheckPasswordAsync(user, logInRequest.Password))
        {
            return Result<AuthenticationResponse>.FromFailed(401, LogInErrorMessage);
        }

        var tokens = GenerateTokens(user);
        await _refreshTokenRepository.Create(tokens.RefreshToken, user.Id, Guid.NewGuid());

        return Result<AuthenticationResponse>.FromSuccess(tokens);
    }

    public async Task<Result<AuthenticationResponse>> Refresh(string refreshToken)
    {
        var storedRefreshToken = await _refreshTokenRepository.GetByToken(refreshToken);
        if (storedRefreshToken is null)
        {
            return Result<AuthenticationResponse>.FromFailed(401, RefreshErrorMessage);
        }

        if (!storedRefreshToken.Valid)
        {
            await _refreshTokenRepository.InvalidateFamily(storedRefreshToken.Family);
            return Result<AuthenticationResponse>.FromFailed(401, RefreshErrorMessage);
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var parsedToken = tokenHandler.ReadJwtToken(refreshToken);
        
        var userId = parsedToken.Claims.FirstOrDefault(claim => claim.Type == "Id");

        if (userId is null)
        {
            await _refreshTokenRepository.InvalidateFamily(storedRefreshToken.Family);
            return Result<AuthenticationResponse>.FromFailed(401, RefreshErrorMessage);
        }

        var user = await _userManager.FindByIdAsync(userId.Value);
        if (user is null)
        {
            await _refreshTokenRepository.InvalidateFamily(storedRefreshToken.Family);
            return Result<AuthenticationResponse>.FromFailed(401, RefreshErrorMessage);
        }
        
        var newTokens = GenerateTokens(user);
        await _refreshTokenRepository.InvalidateToken(storedRefreshToken.Id);
        await _refreshTokenRepository.Create(newTokens.RefreshToken, user.Id, storedRefreshToken.Family);
        
        return Result<AuthenticationResponse>.FromSuccess(newTokens);
    }

    /// <summary>
    /// Generates access and refresh tokens containing user claims
    /// </summary>
    /// <param name="user">The user to generate the tokens for</param>
    /// <returns>Object containing the generated access and refresh tokens</returns>
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
            DateTime.Now.AddMinutes(_environmentSettings.Jwt.AccessTokenLifetimeMinutes),
            new SigningCredentials(accessTokenKey, SecurityAlgorithms.HmacSha256)
        );

        var refreshTokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_environmentSettings.Jwt.RefreshKey));
        var refreshToken = new JwtSecurityToken(
            _environmentSettings.Jwt.Issuer,
            _environmentSettings.Jwt.Audience,
            [claims[0]],
            DateTime.Now,
            DateTime.Now.AddDays(_environmentSettings.Jwt.RefreshTokenLifetimeDays),
            new SigningCredentials(refreshTokenKey, SecurityAlgorithms.HmacSha256)
        );
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        return new AuthenticationResponse(tokenHandler.WriteToken(accessToken), tokenHandler.WriteToken(refreshToken));
    }
}