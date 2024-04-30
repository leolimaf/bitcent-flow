namespace BitcentFlow.Application.DTOs.Usuario;

public record ReadLoginUsuarioDTO(
    bool Authenticated,
    string Created,
    string Expiration,
    string AccessToken,
    string RefreshToken
);