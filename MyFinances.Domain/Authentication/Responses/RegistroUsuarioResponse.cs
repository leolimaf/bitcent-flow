namespace MyFinances.Domain.Authentication.Responses;

public record RegistroUsuarioResponse
{
    public Guid Id { get; init; }
    public string Nome { get; init; }
    public string Sobrenome { get; init; }
    public string Email { get; init; }
}