using Microsoft.AspNetCore.Mvc;
using StocksCompetitionCore.Models;

namespace StocksCompetitionApi.Controllers;

/// <summary>
/// Provides addition controller helper methods
/// </summary>
public class ExtendedControllerBase : Controller
{
    /// <summary>
    /// Converts a failure result object into a http error response object
    /// </summary>
    /// <param name="result">The failure result object to generate response from</param>
    /// <typeparam name="T">The type of the result object</typeparam>
    /// <returns>Error action result matching result error code</returns>
    /// <exception cref="InvalidOperationException">Thrown if a successful result object if given</exception>
    [NonAction]
    protected IActionResult ErrorResponseFromResult<T>(Result<T> result)
    {
        if (result.Success)
        {
            throw new InvalidOperationException("Error response cannot be generated from successful result");
        }
        
        return result.ErrorCode switch
        {
            400 => BadRequest(result.ErrorMessage),
            401 => Unauthorized(result.ErrorMessage),
            404 => NotFound(result.ErrorMessage),
            409 => Conflict(result.ErrorMessage),
            _ => StatusCode(500, result.ErrorMessage)
        };
    }
}