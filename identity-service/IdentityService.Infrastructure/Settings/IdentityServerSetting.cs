using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Infrastructure.Settings;

public class IdentityServerSetting
{
    private readonly IConfigurationManager _configuration;

    public IdentityServerSetting(IConfigurationManager configuration)
    {
        _configuration = configuration;
    }

    public IReadOnlyCollection<ApiScope> GetApiScopes()
    {
        return new[]
        {
            new ApiScope("video-service.full-access"),
            new ApiScope("storage-service.full-access"),
            new ApiScope("interaction-service.full-access")
        };
    }

    public IReadOnlyCollection<ApiResource> GetApiResources()
    {
        return new[]
        {
            new ApiResource("video-service", "Video Service")
            {
                Scopes = { "video-service.full-access" }
            },
            new ApiResource("storage-service", "Storage Service")
            {
                Scopes = { "storage-service.full-access" }
            },
            new ApiResource("interaction-service", "Interaction Service")
            {
                Scopes = { "interaction-service.full-access" }
            }
        };
    }

    public IReadOnlyCollection<Client> GetClients()
    {
        var clientUrl = _configuration.GetValue<string>("WebClient:BaseURL");
        var secret = _configuration.GetValue<string>("WebClient:Secret");

        if (string.IsNullOrEmpty(clientUrl))
        {
            throw new ArgumentException("WebClient:BaseURL is not provided.");
        }
        else if (string.IsNullOrEmpty(secret))
        {
            throw new ArgumentException("WebClient:Secret is not provided.");
        }

        return new Client[]
        {

            new Client()
            {
                ClientName = "WebClient",
                ClientId = "web-client",
                RequireClientSecret = true,
                AllowedGrantTypes = { GrantType.AuthorizationCode },
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "video-service.full-access",
                    "storage-service.full-access",
                    "interaction-service.full-access"
                },
                AlwaysIncludeUserClaimsInIdToken = true,
                ClientSecrets =
                {
                    new Secret(secret.Sha256())
                },
                RedirectUris = { $"{clientUrl}/api/auth/callback/webclient" },
                AllowedCorsOrigins = { clientUrl },
                AllowOfflineAccess = true,
                ClientUri = clientUrl
            }
        };
    }

    public IReadOnlyCollection<IdentityResource> GetIdentityResources()
    {
        return new IdentityResource[]
        {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
        };
    }
}
