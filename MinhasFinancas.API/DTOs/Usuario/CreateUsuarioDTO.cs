using System.ComponentModel.DataAnnotations;

namespace MinhasFinancas.API.DTOs.Usuario;

public class CreateUsuarioDTO
{
    [Required]
    public string Nome { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Senha { get; set; }
}