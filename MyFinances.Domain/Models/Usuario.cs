using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyFinances.Domain.Models;

[Table("USUARIO")]
[Index(nameof(Nome), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class Usuario
{
    [Key]
    [Required]
    [Column("ID")]
    public long Id { get; set; }
    
    [Required,] 
    [Column("NOME")]
    public string Nome { get; set; }
    
    [Required, DataType(DataType.EmailAddress)] 
    [Column("EMAIL")]
    public string Email { get; set; }

    [Required, DataType(DataType.Password)]
    [Column("SENHA_HASH")]
    public string SenhaHash { get; set; }
    
    [Column("TOKEN")]
    public string? Token { get; set; }
    
    [Column("VALIDADE_TOKEN")]
    public DateTime? ValidadeToken { get; set; }
    
    public virtual List<TransacaoFinanceira> TransacaoFinanceiras { get; set; }

}