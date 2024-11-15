using BitcentFlow.Auth.Models;

namespace BitcentFlow.Auth.Extensions;

public static class ConfigExtensions
{
    public static IServiceCollection AddCorsConfigurations(this IServiceCollection services)
    {
        return services.AddCors();
    }
    
    public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
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