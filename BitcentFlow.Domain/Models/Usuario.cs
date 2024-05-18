namespace BitcentFlow.Domain.Models;

public class Usuario
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Celular { get; set; }
}