namespace MyFinances.Domain.Authentication.Requests;

public record LoginUsuarioRequest(
    string Email,
    string Senha
);