using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MyFinances.API.Services.Interfaces;
using MyFinances.Domain.DTOs.TransacaoFinanceira;
using MyFinances.Domain.Models;
using MyFinances.Useful.Date;

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
        // Arrange
        var transacaoDto = new CreateTransacaoDTO
        {
            Descricao = "Aluguel",
            Valor = 930.50,
            Data = DataInterna.ObterHorarioDeBrasilia(),
            Tipo = TipoTransacao.DESPESA
        };
        
        // Act
        var transacao = _transacaoFinanceiraService.AdicionarTransacao(transacaoDto);
        
        // Assert
        Assert.IsType<ReadTransacaoDTO>(transacao);
    }

    [Fact]
    public void TestarListarTransacoesTipoDeRetorno()
    {
        var listaDeTransacoes = _transacaoFinanceiraService.ListarTransacoes();
        Assert.IsType<List<ReadTransacaoDTO>>(listaDeTransacoes);
    }
    
    [Fact(DisplayName = "Testa se as transações retornadas são apenas do usuário autenticado")]
    public void TestarListarTransacoesDeUmUnicoUsuario()
    {
        var listaDeTransacoes = _transacaoFinanceiraService.ListarTransacoes();

        var idsUsuarios = listaDeTransacoes
                                    .GroupBy(t => t.IdUsuario)
                                    .Distinct()
                                    .ToList();
        
        if (idsUsuarios.Count > 1)
            Assert.True(false, "As transações retornadas não são de um único usuário.");
        else
            Assert.True(true);
    }

    [Fact]
    public void TestarObterTransacaoExistentePorId()
    {
        var transacao = _transacaoFinanceiraService.ObterTransacaoPorId(new Guid("6aee466f-f10e-4fa8-94d8-fe02a4c7613f"));
        Assert.NotNull(transacao);
    }
    
    [Fact]
    public void TestarObterTransacaoInexistentePorId()
    {
        var transacao = _transacaoFinanceiraService.ObterTransacaoPorId(new Guid("5b411039-b60d-4f8b-bc55-80eb90af53a3"));
        Assert.Null(transacao);
    }
    
    [Theory]
    [InlineData("6aee466f-f10e-4fa8-94d8-fe02a4c7613f")]
    [InlineData("15901a48-f791-4175-bc4a-e7bac7edd065")] 
    [InlineData("a78377f9-ceb7-4aa7-8b5f-34ff35004754")]
    public void TestarObterTransacaoExistentePorVariosIds(string id)
    {
        var transacao = _transacaoFinanceiraService.ObterTransacaoPorId(new Guid(id));
        Assert.NotNull(transacao);
    }
    
    [Fact]
    public void TestarAtualizarTransacao()
    {
        // ARRANGE
        var transacao = _transacaoFinanceiraService.ObterTransacaoPorId(new Guid("6aee466f-f10e-4fa8-94d8-fe02a4c7613f"));
        
        var transacaoDto = new UpdateTransacaoDTO
        {
            Descricao = transacao.Descricao,
            Valor = 246.75,
            Data = DateTime.Today,
            Tipo = transacao.Tipo
        };

        // ACT
        var result = _transacaoFinanceiraService.AtualizarTransacao(new Guid("6aee466f-f10e-4fa8-94d8-fe02a4c7613f"), transacaoDto);
        
        // ASSERT
        Assert.True(result.IsSuccess);
    }
    
    [Fact(DisplayName = "Testa o atualizar transações utilizando o verbo patch do Http")]
    public void TestarAtualizarTransacaoParcialmente()
    {
        var transacaoDto = new JsonPatchDocument
        {
            Operations =
            {
                new Operation
                {
                    op = "replace",
                    path = "valor",
                    value = 29.50
                },
                new Operation
                {
                    op = "replace",
                    path = "descricao",
                    value = "Lanche"
                }
            }
        };
        
        var result = _transacaoFinanceiraService.AtualizarTransacaoParcialmente(new Guid("15901a48-f791-4175-bc4a-e7bac7edd065"), transacaoDto);
        
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void TestarRemoverTransacao()
    {
        var result = _transacaoFinanceiraService.RemoverTransacao(new Guid("a78377f9-ceb7-4aa7-8b5f-34ff35004754"));
        Assert.True(result.IsSuccess);
    }
}