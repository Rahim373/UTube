using IdentityService.Infrastructure;
using IdentityService.Web;
using UTube.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddServices(builder.Configuration);
}

var app = builder.Build();
{
    app.UseRouting();
    app.UseIdentityServer();
    app.UseStaticFiles();

    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.None,
        Secure = CookieSecurePolicy.Always
    });

    app.UseAuthentication();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}");

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapPrometheus(app);
    });

    app.Run();
}
