using System.ComponentModel.DataAnnotations;
using MinhasFinancas.Domain.Models;

namespace MinhasFinancas.Domain.DTOs.TransacaoFinanceira;

public class ReadTransacaoDTO
{
    public long Id { get; set; }
    
    public string Descricao { get; set; }
    
    public DateTime Data { get; set; }
    
    public double Valor { get; set; }
    
    public TipoTransacao Tipo { get; set; }
    
    [Required] 
    public long IdUsuario { get; set; }
}