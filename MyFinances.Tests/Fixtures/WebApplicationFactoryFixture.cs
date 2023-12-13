using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyFinances.Application.Common.Interfaces;
using MyFinances.Application.Data;
using MyFinances.Infrastructure.Authentication;
using Testcontainers.MsSql;

namespace MyFinances.Tests.Fixtures;

public class WebApplicationFactoryFixture
    : WebApplicationFactory<Program>,
        IAsyncLifetime
{
    private MsSqlContainer _dbContainer = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server").Build();
    private static int QunatidadeInicialDeUsuarios => 2;
    
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

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

            services.AddScoped<IJwtTokenGenarator, JwtTokenGenarator>();
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        await using var scope = Services.CreateAsyncScope();
        var scopedService = scope.ServiceProvider;
        
        var dbContext = scopedService.GetRequiredService<AppDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Usuarios.AddRangeAsync(DataFixture.ObterUsuarios(QunatidadeInicialDeUsuarios, true));
        
        var jwtTokenGenarator = scopedService.GetRequiredService<IJwtTokenGenarator>();
        var dadosToken = jwtTokenGenarator.GerarToken(DataFixture.ObterUsuarios(1).First());
        AccessToken = dadosToken.AccessToken;
        RefreshToken = dadosToken.RefreshToken;
        
        await dbContext.SaveChangesAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}

[CollectionDefinition("Collection Fixture")]
public class CollectionFixture : ICollectionFixture<WebApplicationFactoryFixture> { }