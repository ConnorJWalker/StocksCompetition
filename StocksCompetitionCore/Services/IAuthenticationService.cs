using StocksCompetitionCore.Models;
using StocksCompetitionCore.Models.DataTransferObjects.Requests;
using StocksCompetitionCore.Models.DataTransferObjects.Responses;

namespace StocksCompetitionCore.Services;

/// <summary>
/// 
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Creates a new user in the database and creates authentication tokens containing the user's sign up
    /// information
    /// </summary>
    /// <param name="signUpRequest">Validated user sign up form</param>
    /// <returns>Object containing access and refresh tokens</returns>
    public Task<Result<AuthenticationResponse>> SignUp(SignUpRequest signUpRequest);
}