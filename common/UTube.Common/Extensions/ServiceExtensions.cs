using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using StackExchange.Redis;
using UTube.Common.Settings;

namespace UTube.Common.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            var logSetting = configuration.GetSection(nameof(LogSetting)).Get<LogSetting>();
            var applicationServiceName = configuration.GetValue<string>("ServiceName") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(applicationServiceName))
            {
                throw new ArgumentNullException("ServiceName");
            }
            else if (string.IsNullOrEmpty(logSetting?.ElasticSearchLoggerUrl))
            {
                throw new ArgumentNullException(nameof(LogSetting.ElasticSearchLoggerUrl));

            }

            services.AddSerilog((hostingContext, config) =>
            {
                config
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .Enrich.WithCorrelationId()
                    .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                    .WriteTo.Console()
                    .WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(logSetting.ElasticSearchLoggerUrl))
                        {
                            AutoRegisterTemplate = true,
                            IndexFormat = $"{applicationServiceName}-{DateTime.UtcNow:yyyy-MM}"
                        });
            });

            services.AddHttpContextAccessor();

            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisSetting = configuration.GetSection(nameof(RedisSetting)).Get<RedisSetting>();

            if (redisSetting == null || string.IsNullOrEmpty(redisSetting.ConnectionString))
            {
                throw new ArgumentNullException(nameof(RedisSetting.ConnectionString), "Invalid Redis connection string");
            }

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisSetting.ConnectionString);
            services.AddSingleton(redis);

            return services;
        }

        public static IServiceCollection AddPrometheus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks();
            return services;
        }
    }
}
