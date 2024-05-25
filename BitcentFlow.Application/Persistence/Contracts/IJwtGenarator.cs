using System.Security.Claims;
using BitcentFlow.Application.DTOs.Usuario;
using BitcentFlow.Domain.Models;

namespace BitcentFlow.Application.Persistence.Contracts;

public interface IJwtGenarator
{
    Task<TokenDTO> GerarToken(Usuario usuario);
    Task<TokenDTO> GerarToken(Usuario usuario, IEnumerable<Claim> claims);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}