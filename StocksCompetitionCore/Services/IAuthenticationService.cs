using StocksCompetitionCore.Models;
using StocksCompetitionCore.Models.DataTransferObjects.Requests;
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
}