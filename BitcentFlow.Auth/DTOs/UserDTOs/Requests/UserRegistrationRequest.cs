namespace BitcentFlow.Auth.DTOs.UserDTOs.Requests;

public record UserRegistrationRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public bool IsAcceptTerms { get; set; }
}
