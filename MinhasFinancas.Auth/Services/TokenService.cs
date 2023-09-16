using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MinhasFinancas.Auth.Configuration;
using MinhasFinancas.Auth.Data;
using MinhasFinancas.Auth.DTOs;
using MinhasFinancas.Auth.Models;

namespace MinhasFinancas.Auth.Services;

public class TokenService
{
    private readonly TokenConfiguration _configuration;
    private AppDbContext _context;

    public TokenService(TokenConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public TokenDto? GenerateToken(Usuario usuario)
    {
        Claim[] claims = {
            new(JwtRegisteredClaimNames.UniqueName, usuario.Nome),
            new Claim(ClaimTypes.Role, "UsuarioComum"),
            new("id", usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };
        
        var accessToken = GenerateAccessToken(claims);
        var refreshToken = GenerateRefreshToken();
        
        usuario.Token = refreshToken;
        usuario.ValidadeToken = DateTime.Now.AddDays(_configuration.DaysToExpiry);

        _context.SaveChanges();

        var dataCriacao = DateTime.Now;
        var dataExpiracao = dataCriacao.AddMinutes(_configuration.Minutes);
        
        return new TokenDto(
            true, 
            dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"), 
            dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
            accessToken,
            refreshToken
        );
    }
    
    private string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));
        var signinCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(2), 
            claims: claims, 
            signingCredentials: signinCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

}