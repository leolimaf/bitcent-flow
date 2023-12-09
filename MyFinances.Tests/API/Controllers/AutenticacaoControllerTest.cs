using System.Net;
using System.Net.Http.Json;
using AutoMapper;
using FluentAssertions;
using MyFinances.Domain.Authentication.Requests;
using MyFinances.Domain.Authentication.Responses;
using MyFinances.Tests.Fixtures;
using MyFinances.Tests.Helpers.HttpHelper;

namespace MyFinances.Tests.API.Controllers;

public class AutenticacaoControllerTest : IClassFixture<WebApplicationFactoryFixture>
{
    private readonly WebApplicationFactoryFixture _factory;
    private readonly HttpClient _client;
    private readonly IMapper _mapper;

    public AutenticacaoControllerTest(WebApplicationFactoryFixture factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _mapper = _factory.ConfigureMapper();
    }

    [Fact(DisplayName = "Ao cadastrar um usuário deve ser retornado o usuário cadastrado")]
    public async Task TestarCadastrarUsuario()
    {
        // ARRANGE
        var novoUsuario = DataFixture.ObterUsuarios(1, true).First();
        
        var usuarioRequest = _mapper.Map<RegistroUsuarioRequest>(novoUsuario);
        
        // ACT
        var requisicao = await _client.PostAsync(HttpHelper.UrlsUsuario.Cadastrar, HttpHelper.GetJsonHttpContent(usuarioRequest));
        var retorno = await requisicao.Content.ReadFromJsonAsync<RegistroUsuarioResponse>();
        
        // ASSERT
        requisicao.StatusCode.Should().Be(HttpStatusCode.Created);
        
        Assert.NotNull(retorno);

        retorno.Nome.Should().Be(novoUsuario.Nome);
        retorno.Email.Should().Be(novoUsuario.Email);
        
    }
}