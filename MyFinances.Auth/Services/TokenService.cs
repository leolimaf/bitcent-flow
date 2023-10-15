using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyFinances.Domain.DTOs.Token;
using MyFinances.Domain.Models;
using MyFinances.Auth.Configurations;
using MyFinances.Auth.Data;

namespace MyFinances.Auth.Services;

public class TokenService
{
    private readonly TokenConfiguration _configuration;
    private AppDbContext _context;

    public TokenService(TokenConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public TokenDTO GerarToken(Usuario usuario)
    {
        Claim[] claims = {
            new(JwtRegisteredClaimNames.UniqueName, usuario.Nome),
            new Claim(ClaimTypes.Role, !usuario.IsAdministrador ? "StandardUser" : "Administrator"),
            new("id", usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };
        
        var accessToken = GerarAccessToken(claims);
        var refreshToken = GerarRefreshToken();
        
        usuario.Token = refreshToken;
        usuario.ValidadeToken = DateTime.Now.AddDays(_configuration.DaysToExpiry);

        _context.SaveChanges();

        var dataCriacao = DateTime.Now;
        var dataExpiracao = dataCriacao.AddMinutes(_configuration.Minutes);
        
        return new TokenDTO(
            true, 
            dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"), 
            dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
            accessToken,
            refreshToken
        );
    }
    
    public string GerarAccessToken(IEnumerable<Claim> claims)
    {
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));
        var signinCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(1.5), 
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: claims, 
            signingCredentials: signinCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public string GerarRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
    
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters{
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret)),
            ValidateLifetime = true
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCulture))
            throw new SecurityTokenException("Invalid Token");

        return principal;
    }

    public TokenDTO RetornarTokenAtualizado(string accessToken, string refreshToken)
    {
        DateTime dataCriacao = DateTime.Now;
        DateTime dataExpiracao = dataCriacao.AddMinutes(_configuration.Minutes);
        
        return new TokenDTO(
            true, 
            dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"), 
            dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
            accessToken,
            refreshToken
        );
    }
}