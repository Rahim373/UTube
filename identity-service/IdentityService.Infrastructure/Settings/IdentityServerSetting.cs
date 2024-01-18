using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityService.Infrastructure.Settings;

public static class IdentityServerSetting
{
    public static IReadOnlyCollection<ApiScope> ApiScopes => new[]
    {
        new ApiScope("video-service.full-access")
    };

    public static IReadOnlyCollection<ApiResource> ApiResources => new[]
    {
        new ApiResource("video-service", "Video Service")
        {
            Scopes = { "video-service.full-access" }
        }
    };

    public static IReadOnlyCollection<Client> Clients => new Client[]
    {
        new Client()
        {
            ClientName = "webclient",
            ClientId = "webclient",
            RequireClientSecret = true,
            AllowedGrantTypes = {GrantType.AuthorizationCode},
            AllowedScopes = {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "video-service.full-access"},
            AlwaysIncludeUserClaimsInIdToken = true,
            ClientSecrets =
            {
                new Secret("d7f02357090218c43f6e381745189aeb4ff9ca8e0e65499f3b740e2c51ef2aff".Sha256())
            },
            RedirectUris = { "http://localhost:3000/api/auth/callback/webclient" },
            AllowedCorsOrigins = { "http://localhost:3000" },
            AllowOfflineAccess = true,
            ClientUri = "http://localhost:3000"
        }
    };

    public static IReadOnlyCollection<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
        };
}
