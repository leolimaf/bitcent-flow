using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinances.Application.Persistence.Authentication;
using MyFinances.Application.Persistence.TransacaoFinanceira;
using MyFinances.Application.Services.Interfaces;
using MyFinances.Infrastructure.Authentication;
using MyFinances.Infrastructure.Repositories;

namespace MyFinances.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        
        services.AddScoped<IJwtTokenGenarator,JwtTokenGenarator>();
        services.AddScoped<ITransacaoFinanceiraRepository, TransacaoFinanceiraRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        
        return services;
    }
}