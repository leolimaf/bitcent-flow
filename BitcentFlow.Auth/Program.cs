using System.Text.Json.Serialization;
using BitcentFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("BitcentFlowConnection")).UseLazyLoadingProxies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.MapGet("me", async (ClaimsPrincipal claims, AppDbContext context) =>
// {
//     var userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
//
//     return await context.Users.FindAsync(Guid.Parse(userId));
// }).RequireAuthorization();

app.UseHttpsRedirection();

app.Run();