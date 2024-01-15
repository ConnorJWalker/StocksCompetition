using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StocksCompetitionCore.Models.DataTransferObjects.Requests.Authentication;

/// <summary>
/// Request content format for token refresh requests
/// </summary>
public record RefreshTokenRequest
{
    /// <summary>
    /// Token previously given to refresh access and refresh token
    /// </summary>
    [Required]
    [JsonPropertyName("RefreshToken")]
    public required string Token { get; init; }
}