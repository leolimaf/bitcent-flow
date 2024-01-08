using System.Security.Claims;
using MyFinances.Application.DTOs.Usuario;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Services.Interfaces;

public interface IJwtTokenGenarator
{
    ReadLoginUsuarioDTO GerarToken(Usuario usuario);
    string GerarAccessToken(IEnumerable<Claim> claims);
    string GerarRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    ReadLoginUsuarioDTO RetornarTokenAtualizado(string accessToken, string refreshToken);
}