using System.ComponentModel.DataAnnotations;

namespace MyFinances.Domain.DTOs.Usuario;

public class CredenciaisDTO
{
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    
    [Required,  DataType(DataType.Password)]
    public string Senha { get; set; }
}