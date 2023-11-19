using System.ComponentModel.DataAnnotations;

namespace MyFinances.Domain.DTOs.Usuario;

public record CredenciaisDTO
{
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; init; }
    
    [Required,  DataType(DataType.Password)]
    public string Senha { get; init; }
}