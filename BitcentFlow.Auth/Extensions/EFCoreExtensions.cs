using BitcentFlow.Auth.Context;
using Microsoft.EntityFrameworkCore;

namespace BitcentFlow.Auth.Extensions;

public static class EFCoreExtensions
{
    public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }

}