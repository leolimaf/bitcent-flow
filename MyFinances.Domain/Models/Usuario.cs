using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyFinances.Domain.Models;

[Table("USUARIO")]
[Index(nameof(Email), IsUnique = true)]
public class Usuario
{
    [Key]
    [Required]
    [Column("ID")]
    public Guid Id { get; set; }
    
    [Required, MaxLength(40)] 
    [Column("NOME")]
    public string Nome { get; set; }
    
    [Required, MaxLength(80)] 
    [Column("SOBRENOME")]
    public string Sobrenome { get; set; }
    
    [Required, DataType(DataType.EmailAddress), MaxLength(80)] 
    [Column("EMAIL")]
    public string Email { get; set; }

    [Required, DataType(DataType.Password)]
    [Column("SENHA_HASH")]
    public string SenhaHash { get; set; }
    
    [Required]
    [Column("DATA_DE_NASCIMENTO")]
    public DateTime? DataDeNascimento { get; set; }
    
    [Column("EMAIL_VERIFICADO")] 
    public bool IsEmailVerificado { get; set; }
    
    [Column("TOKEN")]
    public string? Token { get; set; }
    
    [Column("VALIDADE_TOKEN")]
    public DateTime? ValidadeToken { get; set; }
    
    [Column("ADMINISTRADOR")] 
    public bool IsAdministrador { get; set; }
    
    [Required, Column("ID_CONTATO")]
    public Guid IdContato { get; set; }
    
    public virtual Contato Contato { get; set; }
    
    public virtual List<TransacaoFinanceira> TransacaoFinanceiras { get; set; }
    
    [NotMapped]
    public string Senha { get; set; }
}