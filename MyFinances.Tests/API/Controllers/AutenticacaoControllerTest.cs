using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MyFinances.Domain.Authentication.Requests;
using MyFinances.Domain.Authentication.Responses;
using MyFinances.Tests.Fixtures;
using MyFinances.Tests.Helpers.HttpHelper;

namespace MyFinances.Tests.API.Controllers;

[Collection("Collection Fixture")]
public class AutenticacaoControllerTest
{
    private readonly WebApplicationFactoryFixture _factory;
    private readonly HttpClient _client;

    public AutenticacaoControllerTest(WebApplicationFactoryFixture factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_factory.AccessToken}");
    }

    [Fact(DisplayName = "Ao cadastrar um usuário deve ser retornado o usuário cadastrado")]
    public async Task TestarCadastrarUsuario()
    {
        // ARRANGE
        var novoUsuario = DataFixture.ObterUsuarios(1, true).First();
        var usuarioRequest = new RegistroUsuarioRequest(novoUsuario.Nome, novoUsuario.Email, novoUsuario.SenhaNaoCriptografada);
        
        // ACT
        var requisicao = await _client.PostAsync(HttpHelper.UrlsUsuario.Cadastrar, HttpHelper.GetJsonHttpContent(usuarioRequest));
        var retorno = await requisicao.Content.ReadFromJsonAsync<RegistroUsuarioResponse>();
        
        // ASSERT
        requisicao.StatusCode.Should().Be(HttpStatusCode.Created);
        
        Assert.NotNull(retorno);

        retorno.Nome.Should().Be(novoUsuario.Nome);
        retorno.Email.Should().Be(novoUsuario.Email);
    }

    [Fact(DisplayName = "Ao logar um usuário deve ser retornado o access token e o refresh token")]
    public async Task TestarLogarUsuario()
    {
        // ARRANGE
        var usuario = DataFixture.ObterUsuarios(1).First();
        var usuarioRequest = new LoginUsuarioRequest(usuario.Email, usuario.SenhaNaoCriptografada);
        
        // ACT
        var requisicao = await _client.PostAsync(HttpHelper.UrlsUsuario.Logar, HttpHelper.GetJsonHttpContent(usuarioRequest));
        var retorno = await requisicao.Content.ReadFromJsonAsync<LoginUsuarioResponse>();
        
        // ASSERT
        requisicao.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Assert.NotNull(retorno);

        retorno.Authenticated.Should().Be(true);
        retorno.AccessToken.Should().NotBeNullOrWhiteSpace();
        retorno.RefreshToken.Should().NotBeNullOrWhiteSpace();
        
        // TODO: REMOVER DEPENDÊNCIA ENTRE OS TESTES
        _factory.AccessToken = retorno.AccessToken;
        _factory.RefreshToken = retorno.RefreshToken;
    }

    [Fact(DisplayName = "Ao atualizar o token do usuário autenticado deve ser retornado o novo access e refresh token")]
    public async Task TestarAtualizarToken()
    {
        // ARRANGE
        var atualizacaoTokenRequest = new AtualizacaoTokenRequest(_factory.AccessToken, _factory.RefreshToken);

        // ACT
        var requisicao = await _client.PostAsync(HttpHelper.UrlsUsuario.AtualizarToken, HttpHelper.GetJsonHttpContent(atualizacaoTokenRequest));
        var retorno = await requisicao.Content.ReadFromJsonAsync<LoginUsuarioResponse>();

        // ASSERT
        requisicao.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Assert.NotNull(retorno);

        retorno.Authenticated.Should().Be(true);
        retorno.AccessToken.Should().NotBeEquivalentTo(atualizacaoTokenRequest.AccessToken);
        retorno.RefreshToken.Should().NotBeEquivalentTo(atualizacaoTokenRequest.RefreshToken);
        
        // TODO: REMOVER DEPENDÊNCIA ENTRE OS TESTES
        _factory.AccessToken = retorno.AccessToken;
        _factory.RefreshToken = retorno.RefreshToken;
    }
}