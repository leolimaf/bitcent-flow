namespace MyFinances.Domain.Authentication.Requests;

public record RegistroUsuarioRequest
{
    public string Nome { get; init; }
    public string Email { get; init; }
    public string Senha { get; init; }
}