namespace MyFinances.Domain.Authentication.Requests;

public record RegistroUsuarioRequest(
    string Nome,
    string Email,
    string Senha
);