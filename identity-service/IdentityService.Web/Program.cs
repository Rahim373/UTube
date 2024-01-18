using IdentityService.Web;
using IdentityService.Infrastructure;

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
 
    app.Run();
}
