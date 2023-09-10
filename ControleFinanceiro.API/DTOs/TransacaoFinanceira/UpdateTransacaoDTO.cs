using System.ComponentModel.DataAnnotations;
using ControleFinanceiro.API.Models;

namespace ControleFinanceiro.API.DTOs.TransacaoFinanceira;

public class UpdateTransacaoDTO
{
    [Required]
    public string Descricao { get; set; }
    
    [Required]
    public DateTime Data { get; set; }
    
    [Required]
    public decimal Valor { get; set; }
    
    [Required]
    public TipoTransacao Tipo { get; set; }
}