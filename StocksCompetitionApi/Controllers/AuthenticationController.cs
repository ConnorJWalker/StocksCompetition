using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksCompetitionCore.Models.DataTransferObjects.Requests.Authentication;
using StocksCompetitionCore.Models.DataTransferObjects.Responses;
using StocksCompetitionCore.Services;

namespace StocksCompetitionApi.Controllers;

/// <summary>
/// Controller containing endpoints for creating and authenticating users
/// </summary>
[ApiController]
[Route("api/v1/[controller]/[action]")]
public class AuthenticationController : ExtendedControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    
    /// <summary>
    /// Provides values to used dependencies
    /// </summary>
    /// <param name="authenticationService">Service providing authentication functionality</param>
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    /// <summary>
    /// Validates user's signup request, creating new account and jwt if valid
    /// </summary>
    /// <param name="signUpRequest">Object containing user's signup details</param>
    [HttpPost]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest signUpRequest)
    {
        var result = await _authenticationService.SignUp(signUpRequest);
        return result.Success 
            ? Ok(result.Content) 
            : ErrorResponseFromResult(result);
    }

    /// <summary>
    /// Validates user's email and password against stored details, generating a new jwt if valid
    /// </summary>
    /// <param name="logInRequest">Object containing user's log in details</param>
    [HttpPost]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LogIn([FromBody] LogInRequest logInRequest)
    {
        var result = await _authenticationService.LogIn(logInRequest);
        return result.Success 
            ? Ok(result.Content) 
            : ErrorResponseFromResult(result);
    }

    /// <summary>
    /// Invalidates user's jwt replacing with new access and refresh tokens if the provided
    /// refresh token is valid
    /// </summary>
    /// <param name="refreshToken">Token previously given to refresh access and refresh token</param>
    [HttpPost]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshToken)
    {
        var result = await _authenticationService.Refresh(refreshToken.Token);
        return result.Success 
            ? Ok(result.Content) 
            : ErrorResponseFromResult(result);
    }

    /// <summary>
    /// Invalidates users current jwt family and adds the current access token to cache to prevent it from being
    /// used for future authorised endpoint access
    /// </summary>
    /// <param name="refreshToken">The user's current refresh tokens to invalidate</param>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LogOut([FromBody] RefreshTokenRequest refreshToken)
    {
        // Remove "Bearer" from the authorization header
        var accessToken = Request.Headers.Authorization.ToString().Split(" ")[1];
        var result = await _authenticationService.Invalidate(accessToken, refreshToken.Token);
        
        return result.Success ? Ok() : ErrorResponseFromResult(result);
    }
}