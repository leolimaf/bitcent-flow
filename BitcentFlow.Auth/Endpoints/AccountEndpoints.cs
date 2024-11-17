using Microsoft.AspNetCore.Authorization;

namespace BitcentFlow.Auth.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/user-profile", GetUserProfile);
        
        return app;
    }
    
    private static string GetUserProfile()
    {
        return "/user-profile";
    }
}