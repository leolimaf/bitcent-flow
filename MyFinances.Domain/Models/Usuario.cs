using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyFinances.Domain.Models;

[Table("USUARIO")]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(CPF), IsUnique = true)]
public class Usuario
{
    [Key]
    [Required]
    [Column("ID")]
    public Guid Id { get; set; }
    
    [Required, MaxLength(120)] 
    [Column("NOME_COMPLETO")]
    public string NomeCompleto { get; set; }
    
    [Required] 
    [Column("CPF")]
    public int CPF { get; set; }
    
    [Required, DataType(DataType.EmailAddress), MaxLength(120)] 
    [Column("EMAIL")]
    public string Email { get; set; }

    [Required, DataType(DataType.Password), MaxLength(24)]
    [Column("SENHA_HASH")]
    public string SenhaHash { get; set; }
    
    [Required]
    [Column("DATA_DE_NASCIMENTO")]
    public DateTime DataDeNascimento { get; set; }
    
    [Column("EMAIL_VERIFICADO")] 
    public bool IsEmailVerificado { get; set; }
    
    [Column("TOKEN")]
    public string? Token { get; set; }
    
    [Column("VALIDADE_TOKEN")]
    public DateTime? ValidadeToken { get; set; }
    
    [Column("ADMINISTRADOR")] 
    public bool IsAdministrador { get; set; }
    
    [Required, Column("ENDERECO_ID")]
    public Guid EnderecoId { get; set; }
    
    [Required, Column("ID_CONTATO")]
    public Guid IdContato { get; set; }
    
    public virtual Endereco Endereco { get; set; }
    
    public virtual Contato Contato { get; set; }
    
    public virtual List<TransacaoFinanceira> TransacaoFinanceiras { get; set; }
    
    [NotMapped]
    public string SenhaNaoCriptografada { get; set; }
}