using MinhasFinancas.API.DTOs.TransacaoFinanceira;

namespace MinhasFinancas.API.DTOs.Usuario;

public class ReadUsuarioDTO
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public List<ReadTransacaoDTO> TransacaoFinanceiras { get; set; }
}