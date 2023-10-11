using System.ComponentModel.DataAnnotations;
using MinhasFinancas.API.Models;

namespace MinhasFinancas.API.DTOs.TransacaoFinanceira;

public class CreateTransacaoDTO
{
    [Required]
    public string Descricao { get; set; }
    
    [Required]
    public DateTime Data { get; set; }
    
    [Required]
    public double Valor { get; set; }
    
    [Required]
    public TipoTransacao Tipo { get; set; }
    
    [Required] 
    public long IdUsuario { get; set; }
}