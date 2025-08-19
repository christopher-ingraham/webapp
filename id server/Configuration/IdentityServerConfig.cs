using Microsoft.AspNetCore.DataProtection;
using System.Text;

namespace DA.WI.NSGHSM.IdentityServer.Configuration
{
    public static class IdentityServerConfig
    {
        // Simple client configuration for ASP.NET Core Identity + JWT
        public static class ClientSecrets
        {
            public static string ApiClientSecret => "NSGHSM-api-secret-2024";
            public static string PasswordClientSecret => "NSGHSM-password-secret-2024";
        }

        // Simple client validation (for ASP.NET Core Identity)
        public static bool ValidateClient(string clientId, string clientSecret)
        {
            return clientId switch
            {
                "NSGHSM-api-client" => clientSecret == ClientSecrets.ApiClientSecret,
                "NSGHSM-password-client" => clientSecret == ClientSecrets.PasswordClientSecret,
                "NSGHSM-spa" => true,
                _ => false
            };
        }

        // Supported scopes for JWT tokens
        public static class Scopes
        {
            public const string OpenId = "openid";
            public const string Profile = "profile";
            public const string Email = "email";
            public const string Role = "role";
            public const string Api = "NSGHSM-api";
        }

        // Validate if scope is supported
        public static bool IsValidScope(string scope)
        {
            return scope switch
            {
                Scopes.OpenId or Scopes.Profile or Scopes.Email or Scopes.Role or Scopes.Api => true,
                _ => false
            };
        }

        /* 
         * The following code is for Duende IdentityServer only.
         * It requires the Duende.IdentityServer NuGet package and commercial license.
         * Uncomment only if you have the proper license and package installed.
         */

        /*
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> { "role" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("NSGHSM-api", "NSGHSM API Access")
                {
                    UserClaims = new[] { "role", "name", "email", "department" }
                }
            };

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var webUrls = configuration.GetSection("Endpoints:WebUrl").Get<string[]>() ?? Array.Empty<string>();
            
            var clients = new List<Client>();

            // SPA Client for implicit flow
            clients.Add(new Client
            {
                ClientId = "NSGHSM-spa",
                ClientName = "NSGHSM SPA Application",
                
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,
                
                RedirectUris = webUrls.SelectMany(url => new[]
                {
                    $"{url}",
                    $"{url}/silent-renew.html",
                    $"{url}/popup.html",
                    $"{url}/NSGHSM/web",
                    $"{url}/NSGHSM/web/silent-renew.html",
                    $"{url}/NSGHSM/web/popup.html"
                }).ToList(),
                
                PostLogoutRedirectUris = webUrls.ToList(),
                AllowedCorsOrigins = webUrls.ToList(),
                
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "role",
                    "NSGHSM-api"
                },
                
                AccessTokenLifetime = 3600, // 1 hour
                IdentityTokenLifetime = 300, // 5 minutes
                
                AlwaysIncludeUserClaimsInIdToken = true,
                UpdateAccessTokenClaimsOnRefresh = true
            });

            // API Client for machine-to-machine communication
            clients.Add(new Client
            {
                ClientId = "NSGHSM-api-client",
                ClientName = "NSGHSM API Client",
                
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret(HashSecret(ClientSecrets.ApiClientSecret)) },
                
                AllowedScopes = new List<string>
                {
                    "NSGHSM-api"
                },
                
                AccessTokenLifetime = 3600
            });

            // Resource Owner Password Client (for legacy support)
            clients.Add(new Client
            {
                ClientId = "NSGHSM-password-client",
                ClientName = "NSGHSM Password Client",
                
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret(HashSecret(ClientSecrets.PasswordClientSecret)) },
                
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "role",
                    "NSGHSM-api"
                },
                
                AccessTokenLifetime = 3600,
                RefreshTokenUsage = TokenUsage.ReUse,
                AllowOfflineAccess = true
            });

            return clients;
        }
        */
    }
}
