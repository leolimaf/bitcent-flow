using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using BitcentFlow.Application;
using BitcentFlow.Infrastructure;
using BitcentFlow.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseLazyLoadingProxies().UseSqlServer(
        builder.Configuration.GetConnectionString("BitcentFlowConnection")
    );
});

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Cabeçalho de autorização JWT utilizando o Bearer Authentication Scheme.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    
    opts.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    
    if (Debugger.IsAttached)
    {
        opts.AddServer(new OpenApiServer()
        {
            Description = "Local",
            Url = "https://localhost:44330/"
        });
    }
    else
    {
        opts.AddServer(new OpenApiServer()
        {
            Description = "Production",
            Url = "https://bitcent-flow-web-api.com.br"
        });
    }
    opts.SwaggerDoc("v1",new OpenApiInfo
    {
        Title = "Bitcent Flow - Web API",
        Version = "v1",
        Description = "Aplicação web desenvolvida para auxiliar no controle de finanças pessoais",
        Contact = new OpenApiContact
        {
            Name = "Leonardo Lima",
            Url = new Uri("https://leolimaf.github.io/")
        }
    });
    opts.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseApiVersioning();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }