using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhasFinancas.API.Models;

[Table("TRANSACAO_FINANCEIRA")]
public class TransacaoFinanceira
{
    [Key]
    [Required]
    [Column("ID")]
    public long Id { get; set; }
    
    [Required]
    [Column("DESCRICAO")]
    public string Descricao { get; set; }
    
    [Required]
    [Column("DATA")]
    public DateTime Data { get; set; }
    
    [Required]
    [Column("VALOR")]
    public double Valor { get; set; }
    
    [Required]
    [Column("TIPO")]
    public TipoTransacao Tipo { get; set; }

    public virtual Usuario Usuario { get; set; }

    [Required] 
    [Column("ID_USUARIO")]
    public long IdUsuario { get; set; }
}

public enum TipoTransacao
{
    RECEITA,
    DESPESA
}