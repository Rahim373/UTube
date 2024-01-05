using Carter;
using MinimalHelpers.OpenApi;
using OpenTelemetry.Metrics;

namespace VideoService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddCors(o => o.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
        }));
        services.AddGrpcReflection();
        services.AddCarter();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddMissingSchemas();
        });

        services.AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter(config =>
                {
                    config.ScrapeResponseCacheDurationMilliseconds = 1000;
                }));

        return services;
    }
}
