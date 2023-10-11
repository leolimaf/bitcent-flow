using Microsoft.EntityFrameworkCore.Infrastructure;
using MinhasFinancas.API.DTOs.TransacaoFinanceira;
using MinhasFinancas.API.Models;
using MinhasFinancas.API.Services.Interfaces;

namespace MinhasFinancas.Test.Services;

public class TransacaoFinanceiraServiceTest : IClassFixture<TestFixture>
{
    private readonly ITransacaoFinanceiraService _transacaoFinanceiraService;

    public TransacaoFinanceiraServiceTest(TestFixture fixture)
    {
        _transacaoFinanceiraService = fixture.DbContext.GetService<ITransacaoFinanceiraService>();
    }
    
    [Fact]
    public void TestarAdicionarTransacao()
    {
        var transacaoDto = new CreateTransacaoDTO
        {
            Descricao = "Aluguel",
            Valor = 930,
            Data = DateTime.Today,
            Tipo = TipoTransacao.DESPESA
        };
        
        var transacao = _transacaoFinanceiraService.AdicionarTransacao(transacaoDto);
        Assert.IsType<ReadTransacaoDTO>(transacao);
    }

    [Fact]
    public void TestarListarTransacoes()
    {
        // Arrange
        
        // Act
        var listaDeTransacoes = _transacaoFinanceiraService.ListarTransacoes();

        // Assert
        Assert.IsType<List<ReadTransacaoDTO>>(listaDeTransacoes);
    }

    [Fact]
    public void TestarObterTransacaoPorId()
    {
        var transacao = _transacaoFinanceiraService.ObterTransacaoPorId(1);
        Assert.NotNull(transacao);
    }
    
    [Theory]
    [InlineData(1), InlineData(2), InlineData(3)]
    public void TestarObterTransacaoPorVariosIds(int id)
    {
        var transacao = _transacaoFinanceiraService.ObterTransacaoPorId(id);
        Assert.NotNull(transacao);
    }
    
    [Fact]
    public void TestarAtualizarTransacao()
    {
        var transacao = _transacaoFinanceiraService.ObterTransacaoPorId(1);
        
        var transacaoDto = new UpdateTransacaoDTO
        {
            Descricao = transacao.Descricao,
            Valor = 246.75,
            Data = DateTime.Today,
            Tipo = transacao.Tipo
        };

        var result = _transacaoFinanceiraService.AtualizarTransacao(1, transacaoDto);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void TestarRemoverTransacao()
    {
        var result = _transacaoFinanceiraService.RemoverTransacao(3);
        Assert.True(result.IsSuccess);
        
    }
}