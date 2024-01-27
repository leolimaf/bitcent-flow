using Microsoft.Extensions.DependencyInjection;
using MyFinances.Application.Services;
using MyFinances.Application.Services.Interfaces;
using Sieve.Services;

namespace MyFinances.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITransacaoFinanceiraService, TransacaoFinanceiraService>();
        services.AddScoped<IAutenticacaoService, AutenticacaoService>();
        
        services.AddSingleton<SieveProcessor>();

        return services;
    }
}