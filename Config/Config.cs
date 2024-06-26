using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System.Collections.Generic;

namespace SecurityApi.Config;

public static class Config
{
    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            new Client
            {
                ClientId = "client_id",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("client_secret".Sha256()) },
                AllowedScopes = { "api1" }
            }
        };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            new ApiScope("api1", "My API")
        };
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>
        {
            new ApiResource("api1", "My API")
        };
    }

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    }

    public static List<TestUser> GetUsers()
    {
        return new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "alice",
                Password = "password",
                Claims = new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim("role", "Admin")
                }
            },
            new TestUser
            {
                SubjectId = "2",
                Username = "bob",
                Password = "password",
                Claims = new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim("role", "User")
                }
            }
        };
    }
}