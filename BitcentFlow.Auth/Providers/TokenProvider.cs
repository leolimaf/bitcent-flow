using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BitcentFlow.Auth.DTOs.UserDTOs.Responses;
using BitcentFlow.Auth.Interfaces;
using BitcentFlow.Auth.Models;
using BitcentFlow.Auth.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BitcentFlow.Auth.Providers;

public class TokenProvider(IOptions<JwtSettings> jwtOptions) : ITokenProvider
{
    public TokenResponse GerarToken(AppUser usuario, IEnumerable<string> roles)
    {
        var claims = new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, usuario.Id),
            new Claim(ClaimTypes.Email, usuario.Email!),
            new Claim(ClaimTypes.Role, roles.First())
        ]);

        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.Value.ExpirationInMinutes),
            SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtOptions.Value.Issuer,
            Audience = jwtOptions.Value.Audience
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return new TokenResponse
        {
            AccessToken = tokenHandler.WriteToken(securityToken),
            RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            ExpiresIn = DateTime.UtcNow.AddDays(7)
        };
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters{
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = jwtOptions.Value.Issuer,
            ValidAudience = jwtOptions.Value.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret)),
            ClockSkew = TimeSpan.Zero
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
            return new ClaimsPrincipal();

        return principal;
    }
}