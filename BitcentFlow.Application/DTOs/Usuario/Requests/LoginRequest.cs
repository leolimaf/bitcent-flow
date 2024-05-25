using System.ComponentModel.DataAnnotations;

namespace BitcentFlow.Application.DTOs.Usuario.Requests;

public record LoginRequest
{
    [Required, EmailAddress] 
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;
};