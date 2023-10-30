using ProcessService.Worker;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Logging.ClearProviders();

    builder.Services
        .AddDependencies(builder.Configuration);
};

var app = builder.Build();
{
    app.UseSerilogRequestLogging();
    app.MapPrometheusScrapingEndpoint();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseHttpsRedirection();
    app.Run();
}