using MyFinances.Application.DTOs.Contato;

namespace MyFinances.Application.DTOs.Usuario;

public record CreateUsuarioDTO(
    string Nome,
    string Sobrenome,
    string Email,
    string Senha,
    string ConfirmacaoDeSenha,
    DateTime DataDeNascimento,
    ContatoDTO Contato
);