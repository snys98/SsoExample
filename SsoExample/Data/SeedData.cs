using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace SsoExample.Data
{
    public static class SeedData
    {
        public static List<ApiResource> ApiResources { get; set; } = new List<ApiResource>()
        {
            new ApiResource()
            {
                ApiSecrets = new List<Secret>(){new Secret("api") },
                Name = "api",
                Scopes = new List<Scope>(){new Scope("api") },
            }
        };
        public static List<Client> Clients { get; set; } = new List<Client>()
        {
            new Client()
            {
                AllowedScopes = new List<string>(){
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    JwtClaimTypes.Role,
                    "api" },
                RedirectUris = new List<string>(){ "*" },
                PostLogoutRedirectUris = new List<string>(){ "*" },
                AllowedCorsOrigins = new List<string>(){"*"},
                ClientId = "client1",
                ClientName = "client1",
                ClientSecrets = new List<Secret>(){new Secret("client1".Sha256())},
                RequireConsent = false,
                AllowAccessTokensViaBrowser = true,
                AllowedGrantTypes = GrantTypes.Hybrid,
                FrontChannelLogoutUri = "https://localhost:44302/Home/FrontChannelLogout",
                BackChannelLogoutUri = "https://localhost:44302/Home/FrontChannelLogout",
                AllowOfflineAccess = true,
            },
            new Client()
            {
                AllowedScopes = new List<string>(){
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    JwtClaimTypes.Role,
                    "api" },
                RedirectUris = new List<string>(){ "*" },
                PostLogoutRedirectUris = new List<string>(){ "*" },
                AllowedCorsOrigins = new List<string>(){"*"},
                ClientId = "client2",
                ClientName = "client2",
                ClientSecrets = new List<Secret>(){new Secret("client2".Sha256()) },
                RequireConsent = false,
                AllowAccessTokensViaBrowser = true,
                AllowedGrantTypes = GrantTypes.Hybrid,
                FrontChannelLogoutUri = "https://localhost:44303/Home/FrontChannelLogout",
                BackChannelLogoutUri = "https://localhost:44303/Home/FrontChannelLogout",
                AllowOfflineAccess = true,
            },
        };
    }
}
