namespace IdentityService.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddMvcCore();
        services.AddControllersWithViews();

        return services;
    }
}
