using System.ComponentModel.DataAnnotations;

namespace BitcentFlow.Application.DTOs.Usuario;

public record TokenDTO(
    [Required] string AccessToken,
    [Required] string RefreshToken,
    DateTime Criacao,
    DateTime Expiracao
);