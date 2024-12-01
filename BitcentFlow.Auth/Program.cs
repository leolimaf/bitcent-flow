using BitcentFlow.Auth.Endpoints;
using BitcentFlow.Auth.Extensions;
using BitcentFlow.Auth.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwaggerExplorer()
    .InjectDbContext(builder.Configuration)
    .AddAppConfiguration(builder.Configuration)
    .AddIdentityHandlersAndStores()
    .ConfigureIdentityOptions()
    .AddCorsConfigurations()
    .AddIdentityAuth(builder.Configuration);

var app = builder.Build();

app.ConfigurewaggerExplorer()
   .ConfigureCors(builder.Configuration)
   .AddIdentityAuthMiddlewares()
   .AddMiddlewares();

app.MapGroup("/api")
   .MapIdentityApi<AppUser>();

app.MapGroup("/api")
   .MapIdentityUserEndpoints()
   .MapAdminEndpoints()
   .MapAccountEndpoints();

app.Run();