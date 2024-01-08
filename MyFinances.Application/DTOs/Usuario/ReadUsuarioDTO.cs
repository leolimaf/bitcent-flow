namespace MyFinances.Application.DTOs.Usuario;

public record ReadUsuarioDTO(
    Guid Id,
    string Nome,
    string Email
);