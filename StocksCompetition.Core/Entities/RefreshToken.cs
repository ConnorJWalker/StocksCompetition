using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StocksCompetition.Core.Entities;

/// <summary>
/// Token used to generate new access tokens without re-authentication if valid
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// The unique identifier for the refresh token
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    /// <summary>
    /// The id of the user that the refresh token belongs to
    /// </summary>
    [ForeignKey("User")]
    public required int UserId { get; init; }
    
    /// <summary>
    /// The refresh token given to the user
    /// </summary>
    [MaxLength(512)]
    public required string Token { get; init; }
    
    /// <summary>
    /// The family the refresh token belongs to. Generated on log in / sign up and the value
    /// is reused upon refresh with the same family as the original refresh token. This allows for
    /// the invalidation of all tokens belonging to the family in the case that a refresh
    /// token is used more than once
    /// </summary>
    public required Guid Family { get; init; }

    /// <summary>
    /// Set to false when the refresh token has been used to generate another access token or when
    /// the refresh token family has been invalidated 
    /// </summary>
    [DefaultValue(true)]
    public bool Valid { get; init; }

    /// <summary>
    /// The user that the refresh token belongs to
    /// </summary>
    public ApplicationUser User { get; init; } = null!;
}