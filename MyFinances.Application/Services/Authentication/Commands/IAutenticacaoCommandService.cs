using MyFinances.Application.Authentication.Commands.AtualizacaoToken;
using MyFinances.Application.Authentication.Commands.Cadastro;
using MyFinances.Application.Authentication.Common.Responses;

namespace MyFinances.Application.Services.Authentication.Commands;

public interface IAutenticacaoCommandService
{
    Task<RegistroUsuarioResponse> CadastrarUsuario(CadastroCommand usuarioRequest);
    Task<LoginUsuarioResponse?> AtualizarToken(AtualizacaoTokenCommand atualizacaoTokenCommand);
    Task<bool> RevogarToken(string identityName);
}