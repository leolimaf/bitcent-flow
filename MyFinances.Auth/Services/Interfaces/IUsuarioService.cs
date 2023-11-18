using MyFinances.Domain.DTOs.Token;
using MyFinances.Domain.DTOs.Usuario;

namespace MyFinances.Auth.Services.Interfaces;

public interface IUsuarioService
{
    Task<ReadUsuarioDTO> CadastrarUsuario(CreateUsuarioDTO usuarioDto);
    Task<ReadUsuarioDTO> ObterUsuarioPorId(string id);
    Task<TokenDTO?> LogarUsuario(CredenciaisDTO credenciaisDto);
    Task<TokenDTO?> LogarUsuario(TokenValueDTO tokenValueDto);
    Task<bool> RevogarToken(string identityName);
}