﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BitcentFlow.Application.Persistence.TransacaoFinanceira;
using BitcentFlow.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using BitcentFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BitcentFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Configurar criação de migrations em Infrastructure 
        services.AddDbContext<AppDbContext>(opstions =>
                opstions.UseSqlServer(configuration.GetConnectionString("Default"),
                    b => b.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName))
                .UseLazyLoadingProxies());
        
        services.AddCors();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(opts => 
            opts.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        services.AddAuth();
        
        services.AddScoped<ITransacaoFinanceiraRepository, TransacaoFinanceiraRepository>();

        services.AddVersioning();
        
        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddAuthentication();
        
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