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
        DbContext.TransacoesFinanceiras.Add(new() {Id = Guid.NewGuid(), Data = DateTime.Now.AddDays(-1), Descricao = "Conta de luz", Valor = 136.25, Tipo = TipoTransacao.DESPESA, IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")});
        DbContext.TransacoesFinanceiras.Add(new() {Id = Guid.NewGuid(), Data = DateTime.Now.AddHours(-6), Descricao = "Salário", Valor = 2575.00, Tipo = TipoTransacao.RECEITA, IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")});
        DbContext.TransacoesFinanceiras.Add(new() {Id = Guid.NewGuid(), Data = DateTime.Now, Descricao = "Conta de água", Valor = 83.65, Tipo = TipoTransacao.DESPESA, IdUsuario = new Guid("06722053-90c6-416c-adab-3d69fd8f6c0d")});
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}