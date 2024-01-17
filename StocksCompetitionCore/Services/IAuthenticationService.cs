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

    /// <summary>
    /// Invalidates the passed refresh token family and stores the current access token in cache
    /// to prevent it being reused after the log out is requested
    /// </summary>
    /// <param name="accessToken">The token used by the user to access authorised endpoints to invalidate</param>
    /// <param name="refreshToken">The token generated to refresh the given access token to invalidate</param>
    /// <returns>Result object containing a boolean for success</returns>
    public Task<Result<bool>> Invalidate(string accessToken, string refreshToken);

    /// <summary>
    /// Invalidates all refresh tokens belonging to the passes user, storing their id and logout time
    /// in cache to prevent previously issued access tokens being used for authentication
    /// </summary>
    /// <param name="userId">The id of the user to invalid tokens for</param>
    public Task InvalidateAllTokensForUser(int userId);
}