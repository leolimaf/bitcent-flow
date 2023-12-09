using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using MyFinances.Application.Data;
using MyFinances.Application.Services.Interfaces;
using MyFinances.Domain.Models;

namespace MyFinances.Tests.Fixtures;

public abstract class BaseIntegrationTest
    : IClassFixture<WebApplicationFactoryFixture>,
        IDisposable
{
    private readonly IServiceScope _scope;
    private readonly AppDbContext _dbContext;
    protected readonly ITransacaoFinanceiraService TransacaoFinanceiraService;

    private static int QunatidadeInicialDeUsuarios => 2;

    protected BaseIntegrationTest(WebApplicationFactoryFixture factoryFixture)
    {
        _scope = factoryFixture.Services.CreateScope();
        
        _dbContext = _scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var migrator = _dbContext.Database.GetService<IMigrator>();
        migrator.Migrate("MyMigration");
        _dbContext.Database.Migrate();

        TransacaoFinanceiraService = _scope.ServiceProvider.GetRequiredService<ITransacaoFinanceiraService>();

        SeedData();
    }

    private void SeedData()
    {
        // USUÁRIOS
        if (!_dbContext.Usuarios.Any(x => x.Id == new Guid("faae087f-6a08-447e-a311-e43009793f05")))
        {
            _dbContext.Usuarios.Add(new()
            {
                Id = new Guid("faae087f-6a08-447e-a311-e43009793f05"), Nome = "Usuario Um", Email = "usuario1@example.com",
                SenhaHash = "$2a$10$Z33PL3dPB4wCdZapqbP6kO4U7h7IhXk9/AsyhCPRQgYStoiy1fNaa",
                Token = null,
                ValidadeToken = null, 
                IsAdministrador = true
            });
        }

        if (!_dbContext.Usuarios.Any(x => x.Id == new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")))
        {
            _dbContext.Usuarios.Add(new()
            {
                Id = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d"), Nome = "Usuario Dois", Email = "usuario2@example.com",
                SenhaHash = "$2a$10$Z33PL3dPB4wCdZapqbP6kO4U7h7IhXk9/AsyhCPRQgYStoiy1fNaa",
                Token = null,
                ValidadeToken = null,
                IsAdministrador = false
            });
        }
        
        // TRANSAÇÕES FINANCEIRAS
        if (!_dbContext.TransacoesFinanceiras.Any(x => x.Id == new Guid("6aee466f-f10e-4fa8-94d8-fe02a4c7613f")))
        {
            _dbContext.TransacoesFinanceiras.Add(new()
            {
                Id = new Guid("6aee466f-f10e-4fa8-94d8-fe02a4c7613f"), Data = DateTime.Now.AddDays(-1),
                Descricao = "Conta de luz", Valor = 136.25m, Tipo = TipoTransacao.DESPESA,
                IdUsuario = new Guid("faae087f-6a08-447e-a311-e43009793f05")
            });
        }

        if (!_dbContext.TransacoesFinanceiras.Any(x => x.Id == new Guid("15901a48-f791-4175-bc4a-e7bac7edd065")))
        {
            _dbContext.TransacoesFinanceiras.Add(new()
            {
                Id = new Guid("15901a48-f791-4175-bc4a-e7bac7edd065"),
                Data = DateTime.Now.AddDays(-1),
                Descricao = "Almoço", Valor = 26.15m, Tipo = TipoTransacao.DESPESA,
                IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")
            });
        }

        if (!_dbContext.TransacoesFinanceiras.Any(x => x.Id == new Guid("a78377f9-ceb7-4aa7-8b5f-34ff35004754")))
        {
            _dbContext.TransacoesFinanceiras.Add(new()
            {
                Id = new Guid("a78377f9-ceb7-4aa7-8b5f-34ff35004754"), Data = DateTime.Now.AddDays(-1),
                Descricao = "Academia", Valor = 110.00m, Tipo = TipoTransacao.DESPESA,
                IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")
            });
        }

        _dbContext.TransacoesFinanceiras.Add(new()
        {
            Id = Guid.NewGuid(), Data = DateTime.Now.AddHours(-6), Descricao = "Salário", Valor = 2575.00m,
            Tipo = TipoTransacao.RECEITA, IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")
        });
        _dbContext.TransacoesFinanceiras.Add(new()
        {
            Id = Guid.NewGuid(), Data = DateTime.Now, Descricao = "Conta de água", Valor = 83.65m,
            Tipo = TipoTransacao.DESPESA, IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")
        });
        
        _dbContext.SaveChanges();
    }


    public void Dispose()
    {
        _scope?.Dispose();
        _dbContext?.Dispose();
    }
}