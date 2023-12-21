namespace MyFinances.Application.Services.Authentication.Common.Responses;

public record RegistroUsuarioResponse(
    Guid Id,
    string Nome,
    string Email
);