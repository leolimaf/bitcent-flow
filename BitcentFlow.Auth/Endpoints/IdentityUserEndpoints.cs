using System.Security.Claims;
using BitcentFlow.Auth.DTOs.UserDTOs.Requests;
using BitcentFlow.Auth.DTOs.UserDTOs.Responses;
using BitcentFlow.Auth.Interfaces;
using BitcentFlow.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

    private static async Task<IResult> CadastrarUsuario(UserManager<AppUser> userManager, UserRegistrationRequest userRegistrationRequest)
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

    private static async Task<IResult> LogarUsuario(ITokenProvider tokenProvider, UserManager<AppUser> userManager, UserLoginRequest userLoginRequest)
    {
        var usuario = await userManager.FindByEmailAsync(userLoginRequest.Email);
    
        if (usuario is null || !await userManager.CheckPasswordAsync(usuario, userLoginRequest.Password))
            return Results.BadRequest(new TokenResponse{ Message = "Email or password is incorrect."});
        
        var roles = await userManager.GetRolesAsync(usuario);
        
        var tokenResponse = tokenProvider.GerarToken(usuario, roles);
        
        usuario.Token = tokenResponse.RefreshToken;
        usuario.TokenUtcExpiration = tokenResponse.ExpiresIn!.Value;

        await userManager.UpdateAsync(usuario);
    
        return Results.Ok(tokenResponse);
    }
    
    private static async Task<IResult> AtualizarToken(ITokenProvider tokenProvider, UserManager<AppUser> userManager, TokenRequest tokenRequest)
    {
         var principal = tokenProvider.GetPrincipalFromExpiredToken(tokenRequest.AccessToken);
        
        var id = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var usuario = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        
        if (usuario is null || usuario.Token != tokenRequest.RefreshToken || usuario.TokenUtcExpiration <= DateTime.UtcNow)
            return Results.BadRequest(new TokenResponse{ Message = "Invalid or expired refresh token."});

        var roles = await userManager.GetRolesAsync(usuario);
        
        var tokenResponse = tokenProvider.GerarToken(usuario, roles);
        
        usuario.Token = tokenResponse.RefreshToken;
        usuario.TokenUtcExpiration = tokenResponse.ExpiresIn!.Value;
        
        await userManager.UpdateAsync(usuario);
        
        return Results.Ok(tokenResponse);
    }
}