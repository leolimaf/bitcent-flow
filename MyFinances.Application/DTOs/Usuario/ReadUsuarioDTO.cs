using MyFinances.Application.DTOs.Contato;
using MyFinances.Application.DTOs.Endereco;

namespace MyFinances.Application.DTOs.Usuario;

public record ReadUsuarioDTO
{
    public Guid Id { get; init; }
    public string NomeCompleto { get; init; }
    public int CPF { get; init; }
    public string Email { get; init; }
    public DateTime DataDeNascimento { get; init; }
    public EnderecoDTO Endereco { get; init; }
    public ContatoDTO Contato { get; init; }
}