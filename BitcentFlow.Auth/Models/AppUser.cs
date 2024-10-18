using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BitcentFlow.Auth.Models;

public class AppUser : IdentityUser
{
    [PersonalData, Column(TypeName = "nvarchar(150)")]
    public string FullName { get; set; }

    [PersonalData, Column(TypeName = "Date")]
    public DateTime Birthdate { get; set; }
}
