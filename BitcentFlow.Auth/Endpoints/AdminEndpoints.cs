using Microsoft.AspNetCore.Authorization;

namespace BitcentFlow.Auth.Endpoints;

public static class AdminEndpoints
{
    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/AdminOnly", AdminOnly);
        
        return app;
    }

    [Authorize(Roles = "Admin")]
    private static string AdminOnly()
    {
        return "AdminOnly";
    }
}