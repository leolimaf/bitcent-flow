using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitcentFlow.Application.DTOs.Usuario.Requests;

public record RegistrationRequest
{
    [Required] 
    public string Nome { get; set; } = string.Empty;
    
    [Required] 
    public string Sobrenome { get; set; } = string.Empty;
    
    [Required, EmailAddress] 
    public string Email { get; set; } = string.Empty;
    
    [Required, DataType(DataType.Password)] 
    public string Senha { get; set; } = string.Empty;
    
    [Required, DataType(DataType.Password), Compare(nameof(Senha)), NotMapped] 
    public string ConfirmacaoDeSenhaSenha { get; set; } = string.Empty;
}