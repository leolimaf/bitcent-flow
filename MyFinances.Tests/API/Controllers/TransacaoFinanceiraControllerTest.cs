using System.Net;
using System.Net.Http.Json;
using AutoMapper;
using FluentAssertions;
using MyFinances.Domain.DTOs.TransacaoFinanceira;
using MyFinances.Tests.Fixtures;
using MyFinances.Tests.Helpers.HttpHelper;

namespace MyFinances.Tests.API.Controllers;

[Collection("Collection Fixture")]
public class TransacaoFinanceiraControllerTest
{
    
    private readonly WebApplicationFactoryFixture _factory;
    private readonly HttpClient _client;
    private readonly IMapper _mapper;
    
    public TransacaoFinanceiraControllerTest(WebApplicationFactoryFixture factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _mapper = _factory.ConfigureMapper();
    }
    
    [Fact(DisplayName = "Ao adicionar uma transação financeira, a transação deve ser retornada")]
    public async Task TestarAdicionarTransacao()
    {
        // ARRANGE
        var novaTransacao = DataFixture.ObterTransacoes(1, true).First();
        var transacaoDto = _mapper.Map<CreateTransacaoDTO>(novaTransacao);
        
        // ACT
        var requisicao = await _client.PostAsync(HttpHelper.UrlsTransacaoFinanceira.Adicionar, HttpHelper.GetJsonHttpContent(transacaoDto));
        var retorno = await requisicao.Content.ReadFromJsonAsync<ReadTransacaoDTO>();
        
        // ASSERT
        requisicao.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Assert.NotNull(retorno);

        retorno.Descricao.Should().Be(novaTransacao.Descricao);
        retorno.Data.Should().Be(novaTransacao.Data);
        retorno.Valor.Should().Be(novaTransacao.Valor);
        retorno.Tipo.Should().Be(novaTransacao.Tipo);
        retorno.IdUsuario.Should().Be(novaTransacao.IdUsuario);
    }
}