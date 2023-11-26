using MyFinances.Domain.Authentication.Requests;
using MyFinances.Domain.Authentication.Responses;

namespace MyFinances.Application.Services.Interfaces;

public interface IAutenticacaoService
{
    Task<RegistroUsuarioResponse> CadastrarUsuario(RegistroUsuarioRequest usuarioRequest);
    Task<RegistroUsuarioResponse> ObterUsuarioPorId(string id);
    Task<LoginUsuarioResponse?> LogarUsuario(LoginUsuarioRequest loginUsuarioRequest);
    Task<LoginUsuarioResponse?> LogarUsuario(AtualizacaoTokenRequest atualizacaoTokenRequest);
    Task<bool> RevogarToken(string identityName);
}