using System.ComponentModel.DataAnnotations;
using ControleFinanceiro.API.Models;

namespace ControleFinanceiro.API.DTOs.TransacaoFinanceira;

public class ReadTransacaoDTO
{
    public long Id { get; set; }
    
    public string Descricao { get; set; }
    
    public DateTime Data { get; set; }
    
    public decimal Valor { get; set; }
    
    public TipoTransacao Tipo { get; set; }
    
    [Required] 
    public long IdUsuario { get; set; }
}