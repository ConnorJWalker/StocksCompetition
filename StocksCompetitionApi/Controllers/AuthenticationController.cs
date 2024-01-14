using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksCompetitionCore.Models.DataTransferObjects.Requests;
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
    /// Invalidates user's jwt replacing with new token
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> RefreshToken()
    {
        return NoContent();
    }

    /// <summary>
    /// Invalidates users current jwt family
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        return NoContent();
    }
}