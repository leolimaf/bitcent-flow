using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyFinances.Application.Authentication;
using MyFinances.Application.Persistence.Authentication;
using MyFinances.Infrastructure.Authentication;
using MyFinances.Infrastructure.Context;
using Testcontainers.MsSql;

namespace MyFinances.Tests.Fixtures;

public class WebApplicationFactoryFixture
    : WebApplicationFactory<Program>,
        IAsyncLifetime
{
    private MsSqlContainer _dbContainer = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server").Build();
    private static int QuantidadeInicialDeUsuarios => 2;
    
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

        var usuariosIniciais = DataFixture.ObterUsuarios(QuantidadeInicialDeUsuarios);
        
        var jwtTokenGenarator = scopedService.GetRequiredService<IJwtTokenGenarator>();
        var token = jwtTokenGenarator.GerarToken(usuariosIniciais.First());

        usuariosIniciais.First().Token = RefreshToken = token.RefreshToken;
        usuariosIniciais.First().ValidadeToken = DateTime.Parse(token.Expiration);

        AccessToken = token.AccessToken;
        
        await dbContext.Usuarios.AddRangeAsync(usuariosIniciais);
        await dbContext.SaveChangesAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}

[CollectionDefinition(nameof(IntegrationApiTestFixtureCollection))]
public class IntegrationApiTestFixtureCollection : ICollectionFixture<WebApplicationFactoryFixture> { }