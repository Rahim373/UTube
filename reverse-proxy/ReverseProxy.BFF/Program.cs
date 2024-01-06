using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

    builder.Services.AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter(config =>
                {
                    config.ScrapeResponseCacheDurationMilliseconds = 1000;
                }));
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.MapReverseProxy();
    app.MapPrometheusScrapingEndpoint();
    app.UseRouting();
    app.Run();
}
