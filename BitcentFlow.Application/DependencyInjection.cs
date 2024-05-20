using BitcentFlow.Application.Services;
using BitcentFlow.Application.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;

namespace BitcentFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITransacaoFinanceiraService, TransacaoFinanceiraService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        
        services.AddSingleton<SieveProcessor>();

        return services;
    }
}