using BitcentFlow.Application.Interfaces;
using BitcentFlow.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;

namespace BitcentFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITransacaoFinanceiraService, TransacaoFinanceiraService>();
        
        services.AddSingleton<SieveProcessor>();

        return services;
    }
}