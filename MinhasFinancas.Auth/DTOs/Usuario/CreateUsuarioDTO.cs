using System.ComponentModel.DataAnnotations;

namespace MinhasFinancas.Auth.DTOs.Usuario;

public class CreateUsuarioDTO
{
    [Required]
    public string Nome { get; set; }
    
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [Required, DataType(DataType.Password)]
    public string Senha { get; set; }
}