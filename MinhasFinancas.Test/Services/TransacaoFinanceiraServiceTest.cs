using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinhasFinancas.API.Data;
using MinhasFinancas.API.DTOs.TransacaoFinanceira;
using MinhasFinancas.API.Services;
using MinhasFinancas.API.Services.Interfaces;

namespace MinhasFinancas.Test.Services;

public class TransacaoFinanceiraServiceTest : IDisposable
{
    private readonly ITransacaoFinanceiraService _transacaoFinanceiraService;
    private readonly AppDbContext _dbContext;
    
    public TransacaoFinanceiraServiceTest()
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

        // Resolvendo as dependências
        _transacaoFinanceiraService = serviceProvider.GetRequiredService<ITransacaoFinanceiraService>();
        _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
    }

    [Fact]
    public void TestarListarTransacoes()
    {
        // Arrange
        
        // Act
        var listaDeTransacoes = _transacaoFinanceiraService.ListarTransacoes();

        // Assert
        // Assert.NotNull(listaDeTransacoes);
        Assert.IsType<List<ReadTransacaoDTO>>(listaDeTransacoes);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}