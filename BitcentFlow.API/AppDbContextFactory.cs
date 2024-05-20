using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using BitcentFlow.Infrastructure.Context;

namespace BitcentFlow.API;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"),
                b => b.MigrationsAssembly("BitcentFlow.API"))
            .UseLazyLoadingProxies();

        return new AppDbContext(builder.Options);
    }
}