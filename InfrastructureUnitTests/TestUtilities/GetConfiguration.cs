using StocksCompetitionCore.Models.Environment;

namespace InfrastructureUnitTests.TestUtilities;

internal static class GetConfiguration
{
    public static EnvironmentSettings EnvironmentSettings => new EnvironmentSettings
    {
        Jwt = new JwtSettings
        {
            Issuer = "StocksCompetitionIssuer",
            Audience = "StocksCompetitionAudience",
            Key = "UnitTestKeyUnitTestKeyUnitTestKeyUnitTestKeyUnitTestKeyUnitTestKey",
            RefreshKey = "UnitTestRefreshKeyUnitTestRefreshKeyUnitTestRefreshKeyUnitTestRefreshKey",
            AccessTokenLifetimeMinutes = 15,
            RefreshTokenLifetimeDays = 30
        },
        Database = new DatabaseSettings
        {
            Host = "Memory",
            User = "InfrastructureUnitTests",
            Password = "Password",
            Database = "InfrastructureUnitTests"
        }
    };
}