using System.ComponentModel.DataAnnotations;

namespace StocksCompetitionCore.Models.DataTransferObjects.Requests.Authentication;

/// <summary>
/// Request content format for token refresh requests
/// </summary>
public record RefreshRequest
{
    /// <summary>
    /// Token previously given to refresh access and refresh token
    /// </summary>
    [Required]
    public required string RefreshToken { get; init; }
}