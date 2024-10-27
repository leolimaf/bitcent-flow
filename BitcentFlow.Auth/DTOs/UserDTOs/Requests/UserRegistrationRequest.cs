namespace BitcentFlow.Auth.DTOs.UserDTOs.Requests;

public record UserRegistrationRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateTime Birthdate { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string ConfirmPassword { get; init; } = string.Empty;
    public bool AcceptTerms { get; init; }
}
