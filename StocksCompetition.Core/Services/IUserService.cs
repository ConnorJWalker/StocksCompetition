using StocksCompetition.Core.Models;
using StocksCompetition.Core.Models.DataTransferObjects.Requests.Authentication;

namespace StocksCompetition.Core.Services;

/// <summary>
/// Service providing functionality for creating, reading, updating and deleting users
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Creates a user in the database
    /// </summary>
    /// <param name="signUpRequest">Validated user sign up request</param>
    /// <returns>Result object containing true if successful</returns>
    public Task<Result<bool>> Create(SignUpRequest signUpRequest);

    /// <summary>
    /// Searches the database to see if the supplied email is unique
    /// </summary>
    /// <param name="email">The email to search for</param>
    /// <returns>True if the email address was not found in the database</returns>
    public Task<bool> GetEmailIsUnique(string email);
}