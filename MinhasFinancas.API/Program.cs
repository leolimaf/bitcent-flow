using System.Diagnostics;
using System.Reflection;
using MinhasFinancas.API.Data;
using MinhasFinancas.API.Services;
using MinhasFinancas.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("InMemory"));

builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseLazyLoadingProxies().UseSqlServer(
        builder.Configuration.GetConnectionString("MinhasFinancasConnection")
    );
});

builder.Services.AddCors();

builder.Services.AddControllers();
builder.Services.AddScoped<ITransacaoFinanceiraService, TransacaoFinanceiraService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApiVersioning(opts =>
{
    opts.DefaultApiVersion = new ApiVersion(1, 0);
    opts.ReportApiVersions = true;
    opts.AssumeDefaultVersionWhenUnspecified = true;
});
builder.Services.AddVersionedApiExplorer(opts =>
{
    opts.GroupNameFormat = "'v'VVV";
    opts.SubstituteApiVersionInUrl = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
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
            Url = "https://controle-financeiro-web-api.com.br" // TODO: ALTERAR
        });
    }
    opts.SwaggerDoc("v1",new OpenApiInfo
    {
        Title = "Minhas Finanças - Web API",
        Version = "v1",
        Description = "Está aplicação foi desenvolvida com o propósito de auxiliar os usuários a realizarem o gerenciamento de suas transações financeiras, baseando-se nas suas receitas e despesas.",
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

app.UseAuthorization();

app.MapControllers();

app.Run();