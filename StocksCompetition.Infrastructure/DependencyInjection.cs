using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StocksCompetition.Infrastructure.Repositories;
using StocksCompetition.Infrastructure.Services;
using StocksCompetition.Core.Entities;
using StocksCompetition.Core.Models.Environment;
using StocksCompetition.Core.Repositories;
using StocksCompetition.Core.Services;

namespace StocksCompetition.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, EnvironmentSettings environmentSettings)
    {
        AddAuthentication(services, environmentSettings);
        AddServices(services, environmentSettings);
    }
    
    private static void AddAuthentication(IServiceCollection services, EnvironmentSettings environmentSettings)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<StocksCompetitionDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = environmentSettings.Jwt.Issuer,
                ValidAudience = environmentSettings.Jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(environmentSettings.Jwt.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
    }

    private static void AddServices(IServiceCollection services, EnvironmentSettings environmentSettings)
    {
        services.AddMemoryCache();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddSingleton(environmentSettings);
        
        services.AddDbContext<StocksCompetitionDbContext>(options 
            => options.UseNpgsql($"Host={environmentSettings.Database.Host};Database={environmentSettings.Database.Database};Username={environmentSettings.Database.User};Password={environmentSettings.Database.Password}"));
    }
}