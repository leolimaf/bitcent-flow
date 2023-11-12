using System.ComponentModel.DataAnnotations;

namespace MyFinances.Domain.DTOs.Usuario;

public class CreateUsuarioDTO
{
    [Required, MinLength(8), MaxLength(80)]
    public string Nome { get; set; }
    
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [Required, MinLength(7), MaxLength(40), DataType(DataType.Password)]
    public string Senha { get; set; }
}