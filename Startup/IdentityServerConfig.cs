using Duende.IdentityServer;
using Microsoft.Extensions.DependencyInjection;

namespace SecurityApi.Startup;

public static class IdentityServerConfig
{
    public static void AddIdentityServerConfiguration(this IServiceCollection services)
    {
        services.AddIdentityServer()
            .AddDeveloperSigningCredential() // Not for production
            .AddInMemoryClients(global::Config.GetClients())
            .AddInMemoryApiScopes(global::Config.GetApiScopes())
            .AddInMemoryApiResources(global::Config.GetApiResources())
            .AddInMemoryIdentityResources(global::Config.GetIdentityResources())
            .AddTestUsers(global::Config.GetUsers());
    }
}