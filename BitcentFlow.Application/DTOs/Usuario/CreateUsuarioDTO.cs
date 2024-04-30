using BitcentFlow.Application.DTOs.Contato;

namespace BitcentFlow.Application.DTOs.Usuario;

public record CreateUsuarioDTO(
    string Nome,
    string Sobrenome,
    string Email,
    string Senha,
    string ConfirmacaoDeSenha,
    DateTime DataDeNascimento,
    ContatoDTO Contato
);