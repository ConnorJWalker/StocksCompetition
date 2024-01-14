using System.ComponentModel.DataAnnotations;

namespace StocksCompetitionCore.Models.DataTransferObjects.Requests;

/// <summary>
/// Request content format for sign up requests
/// </summary>
public record LogInRequest
{
    /// <summary>
    /// The email address linked to the account to attempt log in for
    /// </summary>
    [Required]
    public required string Email { get; set; }

    /// <summary>
    /// The email address linked to the account to attempt log in for
    /// </summary>
    [Required]
    public required string Password { get; set; }
}