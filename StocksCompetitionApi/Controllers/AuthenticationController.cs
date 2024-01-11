using Microsoft.AspNetCore.Mvc;

namespace StocksCompetitionApi.Controllers;

/// <summary>
/// Controller containing endpoints for creating and authenticating users
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AuthenticationController : Controller
{
    /// <summary>
    /// Validates user's signup request, creating new account and jwt if valid
    /// </summary>
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp()
    {
        return NoContent();
    }

    /// <summary>
    /// Validates user's email and password against stored details, generating a new jwt if valid
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> LogIn()
    {
        return NoContent();        
    }

    /// <summary>
    /// Invalidates user's jwt replacing with new token
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        return NoContent();
    }

    /// <summary>
    /// Invalidates users current jwt family
    /// </summary>
    [HttpPost("logout")]
    public async Task<IActionResult> LogOut()
    {
        return NoContent();
    }
}