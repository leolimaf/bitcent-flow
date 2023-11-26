namespace MyFinances.Domain.Authentication.Requests;

public record RegistroUsuarioRequest( 
    string Nome,
    string Sobrenome,
    string Email,
    string Senha
);