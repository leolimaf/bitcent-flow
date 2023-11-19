using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinances.API.Data;
using MyFinances.API.Services;
using MyFinances.API.Services.Interfaces;
using MyFinances.Domain.Models;
using MyFinances.Useful.Date;
using Sieve.Services;

namespace MyFinances.Test;

public class TestFixture : IDisposable
{
    public AppDbContext DbContext { get; }
    private readonly IConfigurationRoot _configuration;
    private readonly ServiceProvider _serviceProvider;

    public TestFixture()
    {
        // Configuração do serviço
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        _configuration = configurationBuilder.Build();
        
        var serviceCollection = new ServiceCollection();
        
        // serviceCollection.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("InMemoryDatabase"); });

        serviceCollection.AddDbContext<AppDbContext>(options =>
        {
            options.UseLazyLoadingProxies().UseSqlServer(_configuration.GetConnectionString("MyFinancesConnection"));
        });

        serviceCollection.AddSingleton<IHttpContextAccessor, TestHttpContextAccessor>();
        
        serviceCollection.AddScoped<ITransacaoFinanceiraService, TransacaoFinanceiraService>();
        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        serviceCollection.AddSingleton<SieveProcessor>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
        DbContext = _serviceProvider.GetRequiredService<AppDbContext>();

        SeedData();
    }

    private void SeedData()
    {
        // USUÁRIOS
        if (!DbContext.Usuarios.Any(x => x.Id == new Guid("faae087f-6a08-447e-a311-e43009793f05")))
        {
            DbContext.Usuarios.Add(new()
            {
                Id = new Guid("faae087f-6a08-447e-a311-e43009793f05"), Nome = "Usuario Um", Email = "usuario1@example.com",
                SenhaHash = "$2a$10$Z33PL3dPB4wCdZapqbP6kO4U7h7IhXk9/AsyhCPRQgYStoiy1fNaa",
                Token = null,
                ValidadeToken = null, 
                IsAdministrador = true
            });
        }

        if (!DbContext.Usuarios.Any(x => x.Id == new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")))
        {
            DbContext.Usuarios.Add(new()
            {
                Id = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d"), Nome = "Usuario Dois", Email = "usuario2@example.com",
                SenhaHash = "$2a$10$Z33PL3dPB4wCdZapqbP6kO4U7h7IhXk9/AsyhCPRQgYStoiy1fNaa",
                Token = null,
                ValidadeToken = null,
                IsAdministrador = false
            });
        }
        
        // TRANSAÇÕES FINANCEIRAS
        if (!DbContext.TransacoesFinanceiras.Any(x => x.Id == new Guid("6aee466f-f10e-4fa8-94d8-fe02a4c7613f")))
        {
            DbContext.TransacoesFinanceiras.Add(new()
            {
                Id = new Guid("6aee466f-f10e-4fa8-94d8-fe02a4c7613f"), Data = DataInterna.ObterHorarioDeBrasilia().AddDays(-1),
                Descricao = "Conta de luz", Valor = 136.25m, Tipo = TipoTransacao.DESPESA,
                IdUsuario = new Guid("faae087f-6a08-447e-a311-e43009793f05")
            });
        }

        if (!DbContext.TransacoesFinanceiras.Any(x => x.Id == new Guid("15901a48-f791-4175-bc4a-e7bac7edd065")))
        {
            DbContext.TransacoesFinanceiras.Add(new()
            {
                Id = new Guid("15901a48-f791-4175-bc4a-e7bac7edd065"),
                Data = DataInterna.ObterHorarioDeBrasilia().AddDays(-1),
                Descricao = "Almoço", Valor = 26.15m, Tipo = TipoTransacao.DESPESA,
                IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")
            });
        }

        if (!DbContext.TransacoesFinanceiras.Any(x => x.Id == new Guid("a78377f9-ceb7-4aa7-8b5f-34ff35004754")))
        {
            DbContext.TransacoesFinanceiras.Add(new()
            {
                Id = new Guid("a78377f9-ceb7-4aa7-8b5f-34ff35004754"), Data = DataInterna.ObterHorarioDeBrasilia().AddDays(-1),
                Descricao = "Academia", Valor = 110.00m, Tipo = TipoTransacao.DESPESA,
                IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")
            });
        }

        DbContext.TransacoesFinanceiras.Add(new()
        {
            Id = Guid.NewGuid(), Data = DataInterna.ObterHorarioDeBrasilia().AddHours(-6), Descricao = "Salário", Valor = 2575.00m,
            Tipo = TipoTransacao.RECEITA, IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")
        });
        DbContext.TransacoesFinanceiras.Add(new()
        {
            Id = Guid.NewGuid(), Data = DataInterna.ObterHorarioDeBrasilia(), Descricao = "Conta de água", Valor = 83.65m,
            Tipo = TipoTransacao.DESPESA, IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")
        });
        
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}

class TestHttpContextAccessor : IHttpContextAccessor {
    public HttpContext? HttpContext { get; set; }
    
    public TestHttpContextAccessor()
    {
        // Simula um token JWT no HttpContext para testes
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.UniqueName, "usuario2@example.com"),
            new Claim(ClaimTypes.Role, "StandardUser"),
            new(ClaimTypes.NameIdentifier, "06722053-90c6-416c-adab-3d69fd8f6c0d"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        };
    
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);
    
        HttpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
    }
}