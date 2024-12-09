using System.Security.Claims;
using BitcentFlow.Auth.DTOs.UserDTOs.Responses;
using BitcentFlow.Auth.Models;

namespace BitcentFlow.Auth.Interfaces;

public interface ITokenProvider
{
    TokenResponse GerarToken(AppUser usuario, IEnumerable<string> roles);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}