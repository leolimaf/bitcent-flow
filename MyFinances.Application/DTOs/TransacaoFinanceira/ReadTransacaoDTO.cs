using Sieve.Attributes;

namespace MyFinances.Application.DTOs.TransacaoFinanceira;

public record ReadTransacaoDTO
{
    public Guid Id { get; init; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string Descricao { get; init; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime Data { get; init; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public decimal Valor { get; init; }
    
    [Sieve(CanFilter = true)]
    public TipoTransacaoDTO Tipo { get; init; }
    
    public Guid IdUsuario { get; init; }
}