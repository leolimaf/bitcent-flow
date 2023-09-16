using MinhasFinancas.Auth.DTOs;
using MinhasFinancas.Auth.Models;

namespace MinhasFinancas.Auth.Services.Interfaces;

public interface IUsuarioService
{
    Task<ReadUsuarioDTO> CadastrarUsuario(CreateUsuarioDTO usuarioDto);
    Task<ReadUsuarioDTO> ObterUsuarioPorId(long id);
    Task<TokenDto?> LogarUsuario(LoginUsuarioDTO credenciais);
}