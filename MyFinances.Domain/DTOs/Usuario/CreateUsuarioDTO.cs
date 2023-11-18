using System.ComponentModel.DataAnnotations;

namespace MyFinances.Domain.DTOs.Usuario;

public record CreateUsuarioDTO
{
    [Required, MinLength(8), MaxLength(80)]
    public string Nome { get; init; }
    
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; init; }
    
    [Required, MinLength(7), MaxLength(40), DataType(DataType.Password)]
    public string Senha { get; init; }
}