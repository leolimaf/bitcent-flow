namespace MyFinances.Application.DTOs.Endereco;

public record EnderecoDTO
{
    public string Rua { get; init; }
    public string Numero { get; init; }
    public string Complemento { get; init; }
    public string Bairro { get; init; }
    public string Cidade { get; init; }
    public string Estado { get; init; }
    public string CEP { get; init; }
}