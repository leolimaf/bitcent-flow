using System.Security.Claims;
using BitcentFlow.Application.DTOs.Usuario;
using BitcentFlow.Domain.Models;

namespace BitcentFlow.Application.Services.Interfaces;

public interface IJwtTokenGenarator
{
    ReadLoginUsuarioDTO GerarToken(Usuario usuario);
    string GerarAccessToken(IEnumerable<Claim> claims);
    string GerarRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    ReadLoginUsuarioDTO RetornarTokenAtualizado(string accessToken, string refreshToken);
}