using MyFinances.Application.DTOs.Contato;
using MyFinances.Application.DTOs.Endereco;

namespace MyFinances.Application.DTOs.Usuario;

public record CreateUsuarioDTO
{
    public string NomeCompleto { get; set; }
    public int CPF { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string ConfirmacaoDeSenha { get; set; }
    public DateTime DataDeNascimento { get; set; }
    public EnderecoDTO Endereco { get; set; }
    public ContatoDTO Contato { get; set; }
}