namespace StocksCompetitionCore.Models.DataTransferObjects.Responses;

/// <summary>
/// Object containing access token required for clients to authenticate with Api
/// </summary>
/// <param name="AccessToken">Token allowing access to authenticated endpoints</param>
/// <param name="RefreshToken">Token allowing access to token refresh endpoint</param>
public record AuthenticationResponse(string AccessToken, string RefreshToken);