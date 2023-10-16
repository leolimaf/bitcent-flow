using Microsoft.EntityFrameworkCore.Infrastructure;
using MyFinances.API.Services.Interfaces;
using MyFinances.Domain.DTOs.TransacaoFinanceira;
using MyFinances.Domain.Models;

namespace MyFinances.Test.Services;

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
        var transacao = _transacaoFinanceiraService.ObterTransacaoPorId(Guid.NewGuid());
        Assert.NotNull(transacao);
    }
    
    [Theory]
    [InlineData("35d036d0-429e-4102-88ac-08a7af2ba92f"), InlineData("6ada8c22-0235-4595-8fde-34bfab840ce2"), InlineData("2b26dbba-6e17-468e-a411-ff77c0ca2a68")]
    public void TestarObterTransacaoPorVariosIds(string id)
    {
        var transacao = _transacaoFinanceiraService.ObterTransacaoPorId(new Guid(id));
        Assert.NotNull(transacao);
    }
    
    [Fact]
    public void TestarAtualizarTransacao()
    {
        var transacao = _transacaoFinanceiraService.ObterTransacaoPorId(new Guid("35d036d0-429e-4102-88ac-08a7af2ba92f"));
        
        var transacaoDto = new UpdateTransacaoDTO
        {
            Descricao = transacao.Descricao,
            Valor = 246.75,
            Data = DateTime.Today,
            Tipo = transacao.Tipo
        };

        var result = _transacaoFinanceiraService.AtualizarTransacao(new Guid("35d036d0-429e-4102-88ac-08a7af2ba92f"), transacaoDto);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void TestarRemoverTransacao()
    {
        var result = _transacaoFinanceiraService.RemoverTransacao(new Guid("35d036d0-429e-4102-88ac-08a7af2ba92f"));
        Assert.True(result.IsSuccess);
        
    }
}