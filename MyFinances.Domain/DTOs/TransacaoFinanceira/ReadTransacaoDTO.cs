using System.ComponentModel.DataAnnotations;
using MyFinances.Domain.Models;

namespace MyFinances.Domain.DTOs.TransacaoFinanceira;

public class ReadTransacaoDTO
{
    public Guid Id { get; set; }
    
    public string Descricao { get; set; }
    
    public DateTime Data { get; set; }
    
    public double Valor { get; set; }
    
    public TipoTransacao Tipo { get; set; }
    
    [Required] 
    public Guid IdUsuario { get; set; }
}