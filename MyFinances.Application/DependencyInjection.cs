using Microsoft.Extensions.DependencyInjection;
using MyFinances.Application.Services;
using MyFinances.Application.Services.Interfaces;

namespace MyFinances.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITransacaoFinanceiraService, TransacaoFinanceiraService>();
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));

        return services;
    }
}