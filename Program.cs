using Duende.IdentityServer;

var builder = WebApplication.CreateBuilder(args);

// Configure IdentityServer
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential() // Not for production
    .AddInMemoryClients(Config.GetClients())
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddTestUsers(Config.GetUsers());

var app = builder.Build();

app.UseIdentityServer();
app.Run();