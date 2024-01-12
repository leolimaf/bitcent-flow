using MyFinances.Application.DTOs.Contato;
using MyFinances.Application.DTOs.Endereco;

namespace MyFinances.Application.DTOs.Usuario;

public record CreateUsuarioDTO
{
    public string NomeCompleto { get; init; }
    public string CPF { get; init; }
    public string Email { get; init; }
    public string Senha { get; init; }
    public string ConfirmacaoDeSenha { get; init; }
    public DateTime DataDeNascimento { get; init; }
    public EnderecoDTO Endereco { get; init; }
    public ContatoDTO Contato { get; init; }
}