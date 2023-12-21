﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFinances.Application.Common.Interfaces;
using MyFinances.Application.Data;
using MyFinances.Application.Services.Authentication.Common.Responses;
using MyFinances.Application.Services.Interfaces;
using MyFinances.Domain.Exception;
using MyFinances.Domain.Models;

namespace MyFinances.Infrastructure.Authentication;

public class JwtTokenGenarator : IJwtTokenGenarator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtOptions;
    private AppDbContext _context;

    public JwtTokenGenarator(IOptions<JwtSettings> jwtOptions, AppDbContext context, IDateTimeProvider dateTimeProvider)
    {
        _jwtOptions = jwtOptions.Value;
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }

    public LoginUsuarioResponse GerarToken(Usuario usuario)
    {
        Claim[] claims = {
            new(JwtRegisteredClaimNames.UniqueName, usuario.Email),
            new Claim(ClaimTypes.Role, !usuario.IsAdministrador ? "StandardUser" : "Administrator"),
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };
        
        var accessToken = GerarAccessToken(claims);
        var refreshToken = GerarRefreshToken();
        
        usuario.Token = refreshToken;
        usuario.ValidadeToken = _dateTimeProvider.UtcNow.AddDays(_jwtOptions.DaysToExpiry);

        _context.SaveChanges();

        var dataCriacao = _dateTimeProvider.UtcNow;
        var dataExpiracao = dataCriacao.AddMinutes(_jwtOptions.Minutes);
        
        return new LoginUsuarioResponse(
            true, 
            dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"), 
            dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
            accessToken,
            refreshToken
        );
    }
    
    public string GerarAccessToken(IEnumerable<Claim> claims)
    {
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var signinCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            expires: _dateTimeProvider.UtcNow.AddMinutes(60), 
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims, 
            signingCredentials: signinCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public string GerarRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters{
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
            ValidateLifetime = true
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
            throw new MyFinancesException(nameof(token), MyFinancesExceptionType.UNAUTHORIZED, "Access Token inválido.");

        return principal;
    }

    public LoginUsuarioResponse RetornarTokenAtualizado(string accessToken, string refreshToken)
    {
        var dataCriacao = _dateTimeProvider.UtcNow;
        var dataExpiracao = dataCriacao.AddMinutes(_jwtOptions.Minutes);
        
        return new LoginUsuarioResponse(
            true, 
            dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"), 
            dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
            accessToken,
            refreshToken
        );
    }
}