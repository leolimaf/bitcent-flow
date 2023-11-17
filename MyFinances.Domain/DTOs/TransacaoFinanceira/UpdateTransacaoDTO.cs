using System.ComponentModel.DataAnnotations;
using MyFinances.Domain.Models;

namespace MyFinances.Domain.DTOs.TransacaoFinanceira;

public record UpdateTransacaoDTO
{
    [Required, StringLength(80)]
    public string Descricao { get; init; }
    
    [Required]
    public DateTime Data { get; init; }
    
    [Required]
    public decimal Valor { get; init; }
    
    [Required]
    public TipoTransacao Tipo { get; init; }
}