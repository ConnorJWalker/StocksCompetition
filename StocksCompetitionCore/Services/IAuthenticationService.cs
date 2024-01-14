using StocksCompetitionCore.Models;
using StocksCompetitionCore.Models.DataTransferObjects.Requests.Authentication;
using StocksCompetitionCore.Models.DataTransferObjects.Responses;

namespace StocksCompetitionCore.Services;

/// <summary>
/// Service providing functionality for creating user accounts and managing their current
/// authentication state 
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Creates a new user in the database and creates authentication tokens containing the user's sign up
    /// information, if account doesn't already exist
    /// </summary>
    /// <param name="signUpRequest">Validated user sign up form</param>
    /// <returns>Result object containing access and refresh tokens</returns>
    public Task<Result<AuthenticationResponse>> SignUp(SignUpRequest signUpRequest);
 
    /// <summary>
    /// Validates user's log in request and creates authentication tokens containing the user's information
    /// stored in the database
    /// </summary>
    /// <param name="logInRequest">User log in form</param>
    /// <returns>Result object containing access and refresh tokens</returns>
    public Task<Result<AuthenticationResponse>> LogIn(LogInRequest logInRequest);

    /// <summary>
    /// Validates the provided refresh token returning a new access and refresh token if valid
    /// </summary>
    /// <param name="refreshToken">Refresh token to validate and refresh</param>
    /// <returns>Result object containing refreshed access and refresh tokens</returns>
    public Task<Result<AuthenticationResponse>> Refresh(string refreshToken);
}