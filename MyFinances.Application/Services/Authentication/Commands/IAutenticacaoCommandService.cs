using MyFinances.Application.Services.Authentication.Common.Requests;
using MyFinances.Application.Services.Authentication.Common.Responses;

namespace MyFinances.Application.Services.Authentication.Commands;

public interface IAutenticacaoCommandService
{
    Task<RegistroUsuarioResponse> CadastrarUsuario(RegistroUsuarioRequest usuarioRequest);
    Task<LoginUsuarioResponse?> AtualizarToken(AtualizacaoTokenRequest atualizacaoTokenRequest);
    Task<bool> RevogarToken(string identityName);
}