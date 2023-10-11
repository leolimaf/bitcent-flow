using System.ComponentModel.DataAnnotations;
using MinhasFinancas.API.Models;

namespace MinhasFinancas.API.DTOs.TransacaoFinanceira;

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