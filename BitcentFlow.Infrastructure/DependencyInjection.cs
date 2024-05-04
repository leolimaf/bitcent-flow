using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BitcentFlow.Application.Persistence.TransacaoFinanceira;
using BitcentFlow.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace BitcentFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddCors();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        // services.AddAuthorization();
        
        services.AddScoped<ITransacaoFinanceiraRepository, TransacaoFinanceiraRepository>();

        services.AddVersioning();
        
        return services;
    }

    private static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opts =>
        {
            opts.DefaultApiVersion = new ApiVersion(1, 0);
            opts.ReportApiVersions = true;
            opts.AssumeDefaultVersionWhenUnspecified = true;
        });
        services.AddVersionedApiExplorer(opts =>
        {
            opts.GroupNameFormat = "'v'VVV";
            opts.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}