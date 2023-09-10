using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.API.Models;

public class Usuario
{
    [Key]
    [Required]
    public long Id { get; set; }
    
    [Required] 
    public string Nome { get; set; }
    
    [Required] 
    public string Email { get; set; }

    [Required]
    public string Senha { get; set; }
    
}