namespace StocksCompetition.Core.Models.DataTransferObjects.Responses;

/// <summary>
/// Response object storing if a resource is unique
/// </summary>
/// <param name="Unique">True is the requested resource is unique</param>
public record UniqueResponse(bool Unique);