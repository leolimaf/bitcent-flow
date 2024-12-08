using System.Security.Claims;
using BitcentFlow.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BitcentFlow.Auth.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/user-profile", GetUserProfile);
        
        return app;
    }
    
    private static async Task<IResult> GetUserProfile(ClaimsPrincipal user, UserManager<AppUser> userManager)
    {
        var userId = user.Claims.First(x => "sub" == x.Type).Value;
        var userDetails = await userManager.FindByIdAsync(userId);
        return Results.Ok(
            new
            {
                NomeCompleto = userDetails?.FullName,
                userDetails?.Email,
            });
    }
}