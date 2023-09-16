using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhasFinancas.Auth.Models;

[Table("USUARIO")]
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
    [Column("SENHA")]
    public string Senha { get; set; }
    
    [Column("TOKEN")]
    public string? Token { get; set; }
    
    [Column("VALIDADE_TOKEN")]
    public DateTime? ValidadeToken { get; set; }
}