namespace MyFinances.Application.Services.Authentication.Common.Requests;

public record LoginUsuarioRequest(
    string Email,
    string Senha
);