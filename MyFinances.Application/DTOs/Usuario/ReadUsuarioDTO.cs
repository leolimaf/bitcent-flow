using MyFinances.Application.DTOs.Contato;

namespace MyFinances.Application.DTOs.Usuario;

public record ReadUsuarioDTO(
    Guid Id,
    string Nome,
    string Sobrenome,
    string Email,
    DateTime DataDeNascimento,
    ContatoDTO Contato
);