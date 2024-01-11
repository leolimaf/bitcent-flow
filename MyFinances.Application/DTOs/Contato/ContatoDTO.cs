namespace MyFinances.Application.DTOs.Contato;

public record ContatoDTO
{
    public string TelefoneFixo { get; init; }
    
    public string Celular { get; init; }
};