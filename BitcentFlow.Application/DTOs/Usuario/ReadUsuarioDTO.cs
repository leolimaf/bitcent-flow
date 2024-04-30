using BitcentFlow.Application.DTOs.Contato;

namespace BitcentFlow.Application.DTOs.Usuario;

public record ReadUsuarioDTO(
    Guid Id,
    string Nome,
    string Sobrenome,
    string Email,
    DateTime DataDeNascimento,
    ContatoDTO Contato
);