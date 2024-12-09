using BitcentFlow.Auth.Interfaces;
using BitcentFlow.Auth.Models;
using BitcentFlow.Auth.Providers;
using BitcentFlow.Auth.Settings;

namespace BitcentFlow.Auth.Extensions;

public static class ConfigExtensions
{
    public static IServiceCollection AddCorsConfigurations(this IServiceCollection services)
    {
        return services.AddCors();
    }
    
    public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenProvider, TokenProvider>();
        
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        return services;
    }
    
    public static WebApplication ConfigureCors(this WebApplication app, IConfiguration configuration)
    {
        app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        return app;
    }
    
    public static WebApplication AddMiddlewares(this WebApplication app)
    {
        app.UseHttpsRedirection();

        return app;
    }
}