using BitcentFlow.Auth.Context;
using BitcentFlow.Auth.DTOs.UserDTOs.Requests;
using BitcentFlow.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services from IdentityCore
builder.Services
    .AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(options => 
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/signup", async (UserManager<AppUser> userManager, [FromBody] UserRegistrationRequest userRegistrationRequest) => 
{
    if (!userRegistrationRequest.IsAcceptTerms)
        return Results.BadRequest();
    
    AppUser user = new()
    {
        FullName = userRegistrationRequest.FirstName.Trim() + " " + userRegistrationRequest.LastName.Trim(),
        Birthdate = userRegistrationRequest.Birthdate,
        PhoneNumber = userRegistrationRequest.PhoneNumber,
        UserName = userRegistrationRequest.Email,
        Email = userRegistrationRequest.Email,

    };
    var result = await userManager.CreateAsync(user, userRegistrationRequest.Password);

    return result.Succeeded 
    ? Results.Ok(result)
    : Results.BadRequest(result);
});

app.MapGroup("/api").MapIdentityApi<AppUser>();

app.Run();