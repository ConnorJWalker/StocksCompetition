using System.Reflection;
using Microsoft.OpenApi.Models;
using StocksCompetitionCore.Models.Environment;
using StocksCompetitionInfrastructure;

var builder = WebApplication.CreateBuilder(args);

var environmentSettings = builder.Configuration.GetRequiredSection("EnvironmentSettings").Get<EnvironmentSettings>()
    ?? throw new Exception("Environment settings could not be loaded");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Stocks Competition",
        Version = "v1",
        Description = "Competitive stocks trading and socials"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlFilepath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlFilepath);
});

builder.Services.AddInfrastructureServices(environmentSettings);

builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
