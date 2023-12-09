using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyFinances.Application.Data;
using MyFinances.Application.Profiles;
using Testcontainers.MsSql;

namespace MyFinances.Tests.Fixtures;

public class WebApplicationFactoryFixture
    : WebApplicationFactory<Program>,
        IAsyncLifetime
{
    private MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server")
        .Build();
    private static int QunatidadeInicialDeUsuarios => 2;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(_dbContainer.GetConnectionString()));

            // TODO: Descobrir forma mais eficiente de adicionar o HttpContextAccessor ou se livrar dele
            services.AddSingleton<IHttpContextAccessor, TestHttpContextAccessor>();
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
    
    public IMapper ConfigureMapper()
    {
        var config = new MapperConfiguration(x =>
        {
            x.AddProfile<UsuarioProfile>();
            x.AddProfile<TransacaoFinanceiraProfile>();
        });
        
        return config.CreateMapper();
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