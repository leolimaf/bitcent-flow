using System.Diagnostics;
using System.Reflection;
using ControleFinanceiro.API.Business;
using ControleFinanceiro.API.Business.Interfaces;
using ControleFinanceiro.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("InMemory"));

// var connectionString = "ControleFinanceiroConnection";
// builder.Services.AddDbContext<AppDbContext>(opts =>
// {
//     opts.UseLazyLoadingProxies().UseMySql(
//         builder.Configuration.GetConnectionString(connectionString), ServerVersion.AutoDetect(connectionString)
//     );
// });

builder.Services.AddControllers();
builder.Services.AddScoped<ITransacaoFinanceiraBusiness, TransacaoFinanceiraBusiness>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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
        Title = "Controle Financeiro - Web API",
        Version = "v1",
        Description = "API REST desenvolvida para realizar o gerenciamento de transações financeiras",
        Contact = new OpenApiContact
        {
            Name = "Leonardo Lima",
            Url = new Uri("https://github.com/leolimaf/controle-financeiro")
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

app.UseAuthorization();

app.MapControllers();

app.Run();