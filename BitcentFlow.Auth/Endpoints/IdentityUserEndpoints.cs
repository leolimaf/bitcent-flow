using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BitcentFlow.Auth.DTOs.UserDTOs.Requests;
using BitcentFlow.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BitcentFlow.Auth.Endpoints;

public static class IdentityUserEndpoints
{
    public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/signup", CadastrarUsuario);

        app.MapPost("/signin", LogarUsuario);
        
        return app;
    }

    private static async Task<IResult> CadastrarUsuario(UserManager<AppUser> userManager, [FromBody] UserRegistrationRequest userRegistrationRequest)
    {
        if (!userRegistrationRequest.AcceptTerms)
            return Results.BadRequest();
    
        AppUser user = new()
        {
            FullName = userRegistrationRequest.FirstName.Trim() + " " + userRegistrationRequest.LastName.Trim(),
            Birthdate = userRegistrationRequest.Birthdate,
            PhoneNumber = userRegistrationRequest.PhoneNumber,
            UserName = userRegistrationRequest.Email,
            Email = userRegistrationRequest.Email,

        };
        var result = await userManager.CreateAsync(user, userRegistrationRequest.Password);

        return result.Succeeded 
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }

    private static async Task<IResult> LogarUsuario(UserManager<AppUser> userManager, [FromBody] UserLoginRequest userLoginRequest, IOptions<JwtSettings> jwtSettings)
    {
        var user = await userManager.FindByEmailAsync(userLoginRequest.Email);
    
        if (user is null || !await userManager.CheckPasswordAsync(user, userLoginRequest.Password))
            return Results.BadRequest(new { message = "Email or password is incorrect." });
    
        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Secret));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("UserID", user.Id),
            ]),
            Expires = DateTime.UtcNow.AddDays(10),
            SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
    
        return Results.Ok(new{ token });
    }

}