using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BitcentFlow.Auth.DTOs.UserDTOs.Requests;
using BitcentFlow.Auth.Models;
using BitcentFlow.Auth.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BitcentFlow.Auth.Endpoints;

public static class IdentityUserEndpoints
{
    public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/signup", CadastrarUsuario).AllowAnonymous();

        app.MapPost("/signin", LogarUsuario).AllowAnonymous();
        app.MapPost("/refresh-token", AtualizarToken);
        
        return app;
    }

    private static async Task<IResult> CadastrarUsuario(UserManager<AppUser> userManager, [FromBody] UserRegistrationRequest userRegistrationRequest)
    {
        if (!userRegistrationRequest.AcceptTerms)
            return Results.BadRequest();
    
        AppUser appUser = new()
        {
            FullName = userRegistrationRequest.FirstName.Trim() + " " + userRegistrationRequest.LastName.Trim(),
            Birthdate = userRegistrationRequest.Birthdate,
            PhoneNumber = userRegistrationRequest.PhoneNumber,
            UserName = userRegistrationRequest.Email,
            Email = userRegistrationRequest.Email,
            Token = null,
            TokenUtcExpiration = null

        };
        var result = await userManager.CreateAsync(appUser, userRegistrationRequest.Password);
        await userManager.AddToRoleAsync(appUser, "User");

        return result.Succeeded 
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }

    private static async Task<IResult> LogarUsuario(UserManager<AppUser> userManager, [FromBody] UserLoginRequest userLoginRequest, IOptions<JwtSettings> jwtSettings)
    {
        var user = await userManager.FindByEmailAsync(userLoginRequest.Email);
    
        if (user is null || !await userManager.CheckPasswordAsync(user, userLoginRequest.Password))
            return Results.BadRequest(new { message = "Email or password is incorrect." });
    
        var roles = await userManager.GetRolesAsync(user);
        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Secret));
        var claims = new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.Role, roles.First())
        ]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.Value.ExpirationInMinutes),
            SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings.Value.Issuer,
            Audience = jwtSettings.Value.Audience
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        
        var accessToken = tokenHandler.WriteToken(securityToken);
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        
        user.Token = refreshToken;
        user.TokenUtcExpiration = DateTime.UtcNow.AddDays(7);

        await userManager.UpdateAsync(user);
    
        return Results.Ok(new{ accessToken, refreshToken });
    }
    
    private static string AtualizarToken()
    {
        throw new NotImplementedException();
    }

}