using Microsoft.AspNetCore.Identity;

namespace BitcentFlow.Auth.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; }
    public DateTime Birthdate { get; set; }
    public string? Token { get; set; }
    public DateTime? TokenUtcExpiration { get; set; }
}
