using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BitcentFlow.Application.DTOs.Usuario;
using BitcentFlow.Application.Persistence.Contracts;
using BitcentFlow.Domain.Models;
using BitcentFlow.Infrastructure.Context;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace BitcentFlow.Infrastructure.Configurations;

public class JwtGenarator(AppDbContext context, IOptions<JwtSettings> jwtOptions) : IJwtGenarator
{
    private readonly JwtSettings _jwtOptions = jwtOptions.Value;
    
    public async Task<TokenDTO> GerarToken(Usuario usuario)
    {
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        return await GerarToken(usuario, claims);
    }
    
    public async Task<TokenDTO> GerarToken(Usuario usuario, IEnumerable<Claim> claims)
    {
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddDays(1), 
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims, 
            signingCredentials: credenciais
        );
        
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = Guid.NewGuid().ToString();
        
        usuario.Token = refreshToken;
        usuario.ValidadeToken = DateTime.Now.AddDays(_jwtOptions.DaysToExpiry);
        
        await context.SaveChangesAsync();
    
        var dataCriacao = DateTime.Now;
        var dataExpiracao = dataCriacao.AddMinutes(_jwtOptions.Minutes);
    
        return new TokenDTO(accessToken, refreshToken, dataCriacao, dataExpiracao);
    }
    
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters{
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
            ClockSkew = TimeSpan.Zero
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
            return new ClaimsPrincipal();

        return principal;
    }
}