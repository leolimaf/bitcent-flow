using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleFinanceiro.API.Models;

[Table("USUARIO")]
public class Usuario
{
    [Key]
    [Required]
    [Column("ID")]
    public long Id { get; set; }
    
    [Required] 
    [Column("NOME")]
    public string Nome { get; set; }
    
    [Required] 
    [Column("EMAIL")]
    public string Email { get; set; }

    [Required]
    [Column("SENHA")]
    public string Senha { get; set; }
    
    public virtual List<TransacaoFinanceira> TransacaoFinanceiras { get; set; }
}