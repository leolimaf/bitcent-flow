using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyFinances.API.Data;
using MyFinances.API.Services;
using MyFinances.API.Services.Interfaces;
using MyFinances.Domain.Models;

namespace MyFinances.Test;

public class TestFixture : IDisposable
{
    public AppDbContext DbContext { get; }

    public TestFixture()
    {
        // Configuração do serviço
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryDatabase");
        });
        
        serviceCollection.AddScoped<ITransacaoFinanceiraService, TransacaoFinanceiraService>();
        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var serviceProvider = serviceCollection.BuildServiceProvider();
        DbContext = serviceProvider.GetRequiredService<AppDbContext>();

        SeedData();
    }

    private void SeedData()
    {
        DbContext.TransacoesFinanceiras.Add(new() {Id = new Guid("6aee466f-f10e-4fa8-94d8-fe02a4c7613f"), Data = DateTime.Now.AddDays(-1), Descricao = "Conta de luz", Valor = 136.25, Tipo = TipoTransacao.DESPESA, IdUsuario = new Guid("faae087f-6a08-447e-a311-e43009793f05")});
        DbContext.TransacoesFinanceiras.Add(new() {Id = new Guid("15901a48-f791-4175-bc4a-e7bac7edd065"), Data = DateTime.Now.AddDays(-1), Descricao = "Almoço", Valor = 26.15, Tipo = TipoTransacao.DESPESA, IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")});
        DbContext.TransacoesFinanceiras.Add(new() {Id = new Guid("a78377f9-ceb7-4aa7-8b5f-34ff35004754"), Data = DateTime.Now.AddDays(-1), Descricao = "Academia", Valor = 110.00, Tipo = TipoTransacao.DESPESA, IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")});
        DbContext.TransacoesFinanceiras.Add(new() {Id = Guid.NewGuid(), Data = DateTime.Now.AddHours(-6), Descricao = "Salário", Valor = 2575.00, Tipo = TipoTransacao.RECEITA, IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")});
        DbContext.TransacoesFinanceiras.Add(new() {Id = Guid.NewGuid(), Data = DateTime.Now, Descricao = "Conta de água", Valor = 83.65, Tipo = TipoTransacao.DESPESA, IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")});
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}