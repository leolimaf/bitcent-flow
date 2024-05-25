using System.Net;
using System.Net.Http.Json;
using BitcentFlow.Tests.Fixtures;
using BitcentFlow.Tests.Helpers.HttpHelper;
using BitcentFlow.Tests.Mappers;
using FluentAssertions;
using Mapster;
using BitcentFlow.Application.DTOs.Usuario;
using BitcentFlow.Application.DTOs.Usuario.Requests;
using BitcentFlow.Application.DTOs.Usuario.Responses;
using Xunit.Priority;

namespace BitcentFlow.Tests.API.Controllers;

[Collection(nameof(IntegrationApiTestFixtureCollection))]
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class UsuarioControllerTest
{
    private readonly WebApplicationFactoryFixture _factory;
    private readonly HttpClient _client;

    public UsuarioControllerTest(WebApplicationFactoryFixture factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_factory.AccessToken}");
        Mapper.ConfigureMapster();
    }

    [Fact(DisplayName = "Ao cadastrar um usuário deve ser retornado status de sucesso")]
    [Trait("Autenticação e Autorização", "Cadastro"), Priority(1)]
    public async Task AoCadastrarUsuario()
    {
        // GIVEN
        var novoUsuario = DataFixture.ObterUsuarios(1, true).First();
        var usuarioRequest = novoUsuario.Adapt<RegistrationRequest>();
        
        // WHEN
        var requisicao = await _client.PostAsJsonAsync(HttpHelper.UrlsUsuario.Registrar, usuarioRequest);
        var retorno = await requisicao.Content.ReadFromJsonAsync<RegistrationResponse>();
        
        // THEN
        requisicao.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Assert.NotNull(retorno);

        retorno.Cadastrado.Should().Be(true);
        retorno.Mensagem.Should().NotBeNullOrWhiteSpace();
    }

    [Fact(DisplayName = "Ao logar um usuário deve ser retornado o access token e o refresh token")]
    [Trait("Autenticação e Autorização", "Login"), Priority(2)]
    public async Task AoLogarUsuario()
    {
        // GIVEN
        var usuario = DataFixture.ObterUsuarios(1).First();
        var usuarioRequest = usuario.Adapt<LoginRequest>();
        
        // WHEN
        var requisicao = await _client.PostAsJsonAsync(HttpHelper.UrlsUsuario.Logar, usuarioRequest);
        var retorno = await requisicao.Content.ReadFromJsonAsync<LoginResponse>();
        
        // THEN
        requisicao.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Assert.NotNull(retorno);

        retorno.Autenticado.Should().Be(true);
        retorno.Mensagem.Should().NotBeNullOrWhiteSpace();
        retorno.Token.AccessToken.Should().NotBeNullOrWhiteSpace();
        retorno.Token.RefreshToken.Should().NotBeNullOrWhiteSpace();
        
        // TODO: REMOVER DEPENDÊNCIA ENTRE OS TESTES
        _factory.AccessToken = retorno.Token.AccessToken;
        _factory.RefreshToken = retorno.Token.RefreshToken;
    }

    [Fact(DisplayName = "Ao atualizar o token do usuário autenticado deve ser retornado o novo access e refresh token")]
    [Trait("Autenticação e Autorização", "Login"), Priority(3)]
    public async Task AoAtualizarToken()
    {
        // GIVEN
        var atualizacaoTokenRequest = new TokenDTO(_factory.AccessToken, _factory.RefreshToken, default, default);

        // WHEN
        var requisicao = await _client.PostAsJsonAsync(HttpHelper.UrlsUsuario.AtualizarToken, atualizacaoTokenRequest);
        var retorno = await requisicao.Content.ReadFromJsonAsync<LoginResponse>();

        // THEN
        requisicao.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Assert.NotNull(retorno);

        retorno.Autenticado.Should().Be(true);
        retorno.Token.AccessToken.Should().NotBeEquivalentTo(atualizacaoTokenRequest.AccessToken).And.NotBeNullOrWhiteSpace();
        retorno.Token.RefreshToken.Should().NotBeEquivalentTo(atualizacaoTokenRequest.RefreshToken).And.NotBeNullOrWhiteSpace();
        
        // TODO: REMOVER DEPENDÊNCIA ENTRE OS TESTES
        _factory.AccessToken = retorno.Token.AccessToken;
        _factory.RefreshToken = retorno.Token.RefreshToken;
    }

    [Fact(DisplayName = "Ao deslogar o usuário, seu acesso deve ser invalidado")]
    [Trait("Autenticação e Autorização", "Logoff"), Priority(4)]
    public async Task AoRevogarToken()
    {
        // GIVEN 
        
        // WHEN
        var requisicao = await _client.PostAsJsonAsync(HttpHelper.UrlsUsuario.Deslogar, new {});
        
        // THEN
        requisicao.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

}