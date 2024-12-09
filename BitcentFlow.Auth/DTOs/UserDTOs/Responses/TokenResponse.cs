namespace BitcentFlow.Auth.DTOs.UserDTOs.Responses;

public record TokenResponse
{
    public string? Message { get; init; }
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
    public DateTime? ExpiresIn { get; init; }
}