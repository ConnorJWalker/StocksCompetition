using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace StocksCompetitionCore.Models.DataTransferObjects.Requests.Authentication;

/// <summary>
/// Request content format for sign up requests
/// </summary>
public record SignUpRequest
{
    /// <summary>
    /// The email address to be used for authentication and communication
    /// </summary>
    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    /// <summary>
    /// Human readable unique identifier
    /// </summary>
    [Required]
    [MinLength(2)]
    [MaxLength(32)]
    public required string UserName { get; init; }
    
    /// <summary>
    /// Name to publicly display on user's profile and actions
    /// </summary>
    [Required]
    [MinLength(2)]
    [MaxLength(32)]
    public required string DisplayName { get; init; }

    /// <summary>
    /// Colour to render user's chart
    /// </summary>
    [Required]
    [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Display Colour must be a valid hex code")]
    public required string DisplayColour { get; init; }
    
    /// <summary>
    /// Password to be hashed for future authentication
    /// </summary>
    [Required]
    [MinLength(8)]
    public required string Password { get; init; }

    /// <summary>
    /// Confirmation of password field to ensure password was entered correctly
    /// </summary>
    [Required]
    [UsedImplicitly]
    [Compare("Password", ErrorMessage = "Passwords must match")]
    public required string PasswordConfirm { get; init; }
}