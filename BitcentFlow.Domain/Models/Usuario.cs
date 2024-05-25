using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitcentFlow.Domain.Models;

[Table("USUARIO")]
public class Usuario
{
    [Key]
    [Required]
    [Column("ID")]
    public Guid Id { get; set; }
    
    [Required]
    [Column("NOME")]
    public string Nome { get; set; }
    
    [Required]
    [Column("SOBRENOME")]
    public string Sobrenome { get; set; }
    
    [Required]
    [Column("EMAIL"), DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [Required]
    [Column("SENHA")]
    public string SenhaHash { get; set; }
    
    [Required, DataType(DataType.PhoneNumber)]
    public string Celular { get; set; } = string.Empty;
    
    [Column("TOKEN")]
    public string? Token { get; set; }
    
    [Column("VALIDADE_TOKEN")]
    public DateTime? ValidadeToken { get; set; }
}