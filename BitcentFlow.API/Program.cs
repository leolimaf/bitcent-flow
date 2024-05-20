using System.Diagnostics;
using System.Reflection;
using Microsoft.OpenApi.Models;
using BitcentFlow.Application;
using BitcentFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //This is to generate the Default UI of Swagger Documentation
    options.SwaggerDoc("v1",new OpenApiInfo
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
    // To Enable authorization using Swagger (JWT)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Cabeçalho de autorização JWT utilizando o Bearer Authentication Scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
    
    if (Debugger.IsAttached)
    {
        options.AddServer(new OpenApiServer
        {
            Description = "Local",
            Url = "https://localhost:44330/"
        });
    }
    else
    {
        options.AddServer(new OpenApiServer
        {
            Description = "Production",
            // Url = "https://bitcent-flow-web-api.com.br"
        });
    }
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

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

public partial class Program
{
}