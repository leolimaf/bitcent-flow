namespace MyFinances.Application.Services.Authentication.Common.Requests;

public record RegistroUsuarioRequest(
    string Nome,
    string Email,
    string Senha
);