using System.ComponentModel.DataAnnotations;

namespace MyFinances.Domain.DTOs.Token;

public record TokenValueDTO
{
    [Required]
    public string AccessToken { get; init; }
    
    [Required]
    public string RefreshToken { get; init; }
}