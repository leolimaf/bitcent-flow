using ControleFinanceiro.API.Business;
using ControleFinanceiro.API.Business.Interfaces;
using ControleFinanceiro.API.Data;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<IFinancaBusiness, FinancaBusiness>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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