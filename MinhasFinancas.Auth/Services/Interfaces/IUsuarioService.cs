using MinhasFinancas.Auth.DTOs.Token;
using MinhasFinancas.Auth.DTOs.Usuario;

namespace MinhasFinancas.Auth.Services.Interfaces;

public interface IUsuarioService
{
    Task<ReadUsuarioDTO> CadastrarUsuario(CreateUsuarioDTO usuarioDto);
    Task<ReadUsuarioDTO> ObterUsuarioPorId(long id);
    Task<TokenDTO?> LogarUsuario(CredenciaisDTO credenciaisDto);
    Task<TokenDTO> LogarUsuario(TokenValueDTO tokenValueDto);
    Task<bool> RevogarToken(string identityName);
}