using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksCompetition.Core.Models.DataTransferObjects.Requests.Authentication;
using StocksCompetition.Core.Models.DataTransferObjects.Responses;
using StocksCompetition.Core.Services;

namespace StocksCompetition.Api.Controllers;

/// <summary>
/// Controller providing create, read update and delete endpoints for users
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ExtendedControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Provides values to used dependencies
    /// </summary>
    /// <param name="userService">Service providing user table functionality</param>
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    /// <summary>
    /// Validates and stores the new user account into the database
    /// </summary>
    /// <param name="signUpRequest">Object containing user's signup details</param>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] SignUpRequest signUpRequest)
    {
        var result = await _userService.Create(signUpRequest);
        return result.Success ? Ok() : ErrorResponseFromResult(result);
    }

    /// <summary>
    /// Determines if the provided email address is unique against stored users
    /// </summary>
    /// <param name="email">The email address to search for</param>
    [HttpGet("Unique")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UniqueResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> EmailIsUnique([FromQuery] string email) => Ok(new UniqueResponse(await _userService.GetEmailIsUnique(email)));
}