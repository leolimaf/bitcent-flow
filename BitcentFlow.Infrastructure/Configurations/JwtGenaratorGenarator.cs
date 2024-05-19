using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BitcentFlow.Application.Persistence.Contracts;
using BitcentFlow.Domain.Models;
using BitcentFlow.Infrastructure.Context;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace BitcentFlow.Infrastructure.Configurations;

public class JwtGenaratorGenarator(AppDbContext context, IOptions<JwtSettings> jwtOptions) : IJwtGenarator
{
    private readonly JwtSettings _jwtOptions = jwtOptions.Value;
    
    public string GerarToken(Usuario usuario)
    {
        Claim[] claims = {
            new(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };
        
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddDays(1), 
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims, 
            signingCredentials: credenciais
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}