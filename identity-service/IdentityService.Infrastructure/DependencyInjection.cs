using IdentityServer4.Configuration;
using IdentityService.Data.Models;
using IdentityService.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        #region Mongo Setting with AspNetIdentity

        var mongoDbsetting = configuration.GetSection(nameof(MongoDbSetting)).Get<MongoDbSetting>();

        services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(mongoDbsetting?.ConnectionString, mongoDbsetting?.DatabaseName);

        #endregion

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
            .AddInMemoryApiScopes(IdentityServerSetting.ApiScopes)
            .AddInMemoryApiResources(IdentityServerSetting.ApiResources)
            .AddInMemoryClients(IdentityServerSetting.Clients)
            .AddInMemoryIdentityResources(IdentityServerSetting.IdentityResources)
            .AddDeveloperSigningCredential();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        return services;
    }
}
