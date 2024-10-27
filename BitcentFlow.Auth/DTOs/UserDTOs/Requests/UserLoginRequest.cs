namespace BitcentFlow.Auth.DTOs.UserDTOs.Requests;

public record UserLoginRequest
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}