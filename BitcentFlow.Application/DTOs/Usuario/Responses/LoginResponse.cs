namespace BitcentFlow.Application.DTOs.Usuario.Responses;

public record LoginResponse(
    bool Autenticado,
    string Mensagem = null!,
    TokenDTO Token = null!
);