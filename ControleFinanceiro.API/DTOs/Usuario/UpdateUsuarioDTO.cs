using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.API.DTOs.Usuario;

public class UpdateUsuarioDTO
{
    [Required]
    public string Nome { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Senha { get; set; }
}