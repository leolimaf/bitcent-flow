using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyFinances.Application.Data;
using Testcontainers.MsSql;

namespace MyFinances.Tests.Fixtures;

public class IntegrationTestWebAppFactory
    : WebApplicationFactory<Program>,
        IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server")
        .WithPassword("1q2w3e4r%")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptorType = typeof(DbContextOptions<AppDbContext>);

            var descriptor = services.SingleOrDefault(s => s.ServiceType == descriptorType);

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(_dbContainer.GetConnectionString()));

            // TODO: Descobrir forma mais eficiente de adicionar o HttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, TestHttpContextAccessor>();
        });
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}

class TestHttpContextAccessor : IHttpContextAccessor {
    public HttpContext? HttpContext { get; set; }
    
    public TestHttpContextAccessor()
    {
        // Simula um token JWT no HttpContext para testes
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.UniqueName, "usuario2@example.com"),
            new Claim(ClaimTypes.Role, "StandardUser"),
            new(ClaimTypes.NameIdentifier, "06722053-90c6-416c-adab-3d69fd8f6c0d"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        };
    
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);
    
        HttpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
    }
}