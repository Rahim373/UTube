using IdentityServer4.Configuration;
using IdentityService.Data.Models;
using IdentityService.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UTube.Common.Extensions;
using UTube.Common.Settings;

namespace IdentityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        var mongoDbsetting = configuration.GetSection(nameof(MongoDbSetting)).Get<MongoDbSetting>();

        services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(mongoDbsetting?.ConnectionString, mongoDbsetting?.DatabaseName);

        var identityServerSetting = new IdentityServerSetting(configuration);

        services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseErrorEvents = true;
                options.UserInteraction = new UserInteractionOptions
                {
                    LogoutUrl = "/Authentication/Logout",
                    LoginUrl = "/Authentication/Login",
                    LoginReturnUrlParameter = "returnUrl"
                };
            })
            .AddAspNetIdentity<ApplicationUser>()
            .AddInMemoryApiScopes(identityServerSetting.GetApiScopes())
            .AddInMemoryApiResources(identityServerSetting.GetApiResources())
            .AddInMemoryClients(identityServerSetting.GetClients())
            .AddInMemoryIdentityResources(identityServerSetting.GetIdentityResources())
            .AddDeveloperSigningCredential();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        services.AddSerilog(configuration);
        services.AddPrometheus(configuration);

        return services;
    }
}
