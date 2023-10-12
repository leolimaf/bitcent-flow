using System.ComponentModel.DataAnnotations;

namespace MinhasFinancas.Domain.DTOs.Usuario;

public class CreateUsuarioDTO
{
    [Required]
    public string Nome { get; set; }
    
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [Required, DataType(DataType.Password)]
    public string Senha { get; set; }
}