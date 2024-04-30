using BitcentFlow.Application.DTOs.Token;
using BitcentFlow.Application.DTOs.Usuario;

namespace BitcentFlow.Application.Services.Interfaces;

public interface IAutenticacaoService
{
    Task<ReadUsuarioDTO> Cadastrar(CreateUsuarioDTO usuarioDto);
    Task<ReadUsuarioDTO> ObterPorId(string id);
    Task<ReadLoginUsuarioDTO> Logar(LoginUsuarioDTO usuarioDto);
    Task<ReadLoginUsuarioDTO> AtualizarToken(TokenDTO tokenDto);
    Task<bool> Deslogar(string email);
}