using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace UTube.Common.DependencyInjection
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticSearchLoggerUrl = configuration.GetValue<string>("ElasticSearch:LoggerUrl") ?? string.Empty;
            var applicationServiceName = configuration.GetValue<string>("ServiceName") ?? string.Empty;

            services.AddSerilog((hostingContext, config) =>
            {
                config
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .Enrich.WithCorrelationId()
                    .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                    .WriteTo.Console()
                    .WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(elasticSearchLoggerUrl))
                        {
                            AutoRegisterTemplate = true,
                            IndexFormat = $"{applicationServiceName}-{DateTime.UtcNow:yyyy-MM}"
                        });
            });

            services.AddHttpContextAccessor();

            return services;
        }

        public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
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

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Redis");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Connection string \"Redis\" is defined in the appsettings.json");
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
            });

            return services;
        }
    }
}
