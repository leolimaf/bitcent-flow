using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.API.Models;

public class TransacaoFinanceira
{
    [Key]
    [Required]
    public long Id { get; set; }
    
    [Required]
    public string Descricao { get; set; }
    
    [Required]
    public DateTime Data { get; set; }
    
    [Required]
    public decimal Valor { get; set; }
    
    [Required]
    public TipoTransacao Tipo { get; set; }
}

public enum TipoTransacao
{
    RECEITA,
    DESPESA
}