using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace StocksCompetition.Core.Entities;

/// <summary>
/// Extended IdentityUser to contain additional stocks competition display information specific
/// to individual users, as well as admin status
/// </summary>
public class ApplicationUser : IdentityUser<int>
{
    /// <summary>
    /// Name to publicly display on user's profile and actions
    /// </summary>
    [MaxLength(32)]
    public required string DisplayName { get; init; }

    /// <summary>
    /// Profile picture url linked to user
    /// </summary>
    [MaxLength(256)]
    public required string ProfilePicture { get; init; }

    /// <summary>
    /// Colour to render user's chart
    /// </summary>
    [MaxLength(8)]
    public required string DisplayColour { get; init; }

    /// <summary>
    /// True if user should be given access to admin features
    /// </summary>
    [DefaultValue(false)]
    public bool IsAdmin { get; init; }
    
    /// <summary>
    /// Collection of refresh tokens belonging to the user
    /// </summary>
    public IEnumerable<RefreshToken>? RefreshTokens { get; init; }
}