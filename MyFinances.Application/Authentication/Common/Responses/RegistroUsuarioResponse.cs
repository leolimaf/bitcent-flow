namespace MyFinances.Application.Authentication.Common.Responses;

public record RegistroUsuarioResponse(
    Guid Id,
    string Nome,
    string Email
);