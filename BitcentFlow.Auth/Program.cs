using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BitcentFlow.Auth.Context;
using BitcentFlow.Auth.DTOs.UserDTOs.Requests;
using BitcentFlow.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

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

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(y =>
{
    y.SaveToken = false;
    y.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();

app.UseHttpsRedirection();

app.MapPost("/api/signup", async (UserManager<AppUser> userManager, [FromBody] UserRegistrationRequest userRegistrationRequest) => 
{
    if (!userRegistrationRequest.AcceptTerms)
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

app.MapPost("/api/signin", async (UserManager<AppUser> userManager, [FromBody] UserLoginRequest userLoginRequest) =>
{
    var user = await userManager.FindByEmailAsync(userLoginRequest.Email);
    
    if (user is null || !await userManager.CheckPasswordAsync(user, userLoginRequest.Password))
        return Results.BadRequest(new { message = "Email or password is incorrect." });
    
    var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!));
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity([
            new Claim("UserID", user.Id),
        ]),
        Expires = DateTime.UtcNow.AddDays(10),
        SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
    var token = tokenHandler.WriteToken(securityToken);
    
    return Results.Ok(token);
});

app.MapGroup("/api").MapIdentityApi<AppUser>();

app.Run();