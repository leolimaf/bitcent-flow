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
            .UseLazyLoadingProxies()
            .UseSqlServer(configuration.GetConnectionString("BitcentFlowConnection"),
                b => b.MigrationsAssembly("BitcentFlow.API"));

        return new AppDbContext(builder.Options);
    }
}