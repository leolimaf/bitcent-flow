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
        DbContext.TransacoesFinanceiras.Add(new() {Id = 1, Data = DateTime.Now.AddDays(-1), Descricao = "Conta de luz", Valor = 136.25, Tipo = TipoTransacao.DESPESA, IdUsuario = 1});
        DbContext.TransacoesFinanceiras.Add(new() {Id = 2, Data = DateTime.Now.AddHours(-6), Descricao = "Salário", Valor = 2575.00, Tipo = TipoTransacao.RECEITA, IdUsuario = 1});
        DbContext.TransacoesFinanceiras.Add(new() {Id = 3, Data = DateTime.Now, Descricao = "Conta de água", Valor = 83.65, Tipo = TipoTransacao.DESPESA, IdUsuario = 1});
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}