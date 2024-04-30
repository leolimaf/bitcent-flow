namespace BitcentFlow.Application.DTOs.Token;

public record TokenDTO(
    string AccessToken,
    string RefreshToken
);