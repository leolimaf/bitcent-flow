using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyFinances.Application.Data;
using Testcontainers.MsSql;

namespace MyFinances.Tests.Fixtures;

public class WebApplicationFactoryFixture
    : WebApplicationFactory<Program>,
        IAsyncLifetime
{
    private MsSqlContainer _dbContainer = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server").Build();
    private static int QunatidadeInicialDeUsuarios => 2;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = _dbContainer.GetConnectionString();
        
        base.ConfigureWebHost(builder);
        
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(connectionString));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var scopedService = scope.ServiceProvider;
        
        var dbContext = scopedService.GetRequiredService<AppDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Usuarios.AddRangeAsync(DataFixture.ObterUsuarios(QunatidadeInicialDeUsuarios));
        await dbContext.SaveChangesAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}

[CollectionDefinition("Collection Fixture")]
public class CollectionFixture : ICollectionFixture<WebApplicationFactoryFixture> { }