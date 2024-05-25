using System.Net;
using System.Net.Http.Json;
using BitcentFlow.Tests.Fixtures;
using BitcentFlow.Tests.Helpers.HttpHelper;
using FluentAssertions;
using Mapster;
using BitcentFlow.Application.DTOs.TransacaoFinanceira;

namespace BitcentFlow.Tests.API.Controllers;

[Collection(nameof(IntegrationApiTestFixtureCollection))]
public class TransacaoFinanceiraControllerTest
{
    
    private readonly WebApplicationFactoryFixture _factory;
    private readonly HttpClient _client;
    
    public TransacaoFinanceiraControllerTest(WebApplicationFactoryFixture factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }
    
    [Fact(Skip = "Necessário terminar de configurar o ambiente e terminar os teste do controller de autenticação", DisplayName = "Ao adicionar uma transação financeira, a transação deve ser retornada")]
    public async Task TestarAdicionarTransacao()
    {
        // ARRANGE
        var novaTransacao = DataFixture.ObterTransacoes(1, true).First();
        var transacaoDto = novaTransacao.Adapt<CreateTransacaoDTO>();
        
        // ACT
        var requisicao = await _client.PostAsJsonAsync(HttpHelper.UrlsTransacaoFinanceira.Adicionar, transacaoDto);
        var retorno = await requisicao.Content.ReadFromJsonAsync<ReadTransacaoDTO>();
        
        // ASSERT
        requisicao.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Assert.NotNull(retorno);

        retorno.Descricao.Should().Be(novaTransacao.Descricao);
        retorno.Data.Should().Be(novaTransacao.Data);
        retorno.Valor.Should().Be(novaTransacao.Valor);
        // retorno.Tipo.Should().Be(novaTransacao.Tipo);
        // retorno.IdUsuario.Should().Be(novaTransacao.IdUsuario);
    }
}