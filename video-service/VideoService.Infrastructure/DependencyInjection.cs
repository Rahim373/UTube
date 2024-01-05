using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using VideoService.Application.Consumers;
using VideoService.Application.Context;
using VideoService.Infrastructure.Services;
using VideoService.Infrastructure.Settings;

namespace VideoService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        #region MassTransit

        var rabbitMQSetting = new RabbitMQSetting();
        configuration.GetSection(nameof(RabbitMQSetting)).Bind(rabbitMQSetting);
        services.AddSingleton(rabbitMQSetting);

        services.AddMassTransit(config =>
        {
            config.AddConsumer<VideoUploadedEventConsumer>();

            config.SetKebabCaseEndpointNameFormatter();

            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMQSetting.Endpoint, rabbitMQSetting.Port, rabbitMQSetting.VirtualHost, h =>
                {
                    h.Username(rabbitMQSetting.Username);
                    h.Password(rabbitMQSetting.Password);
                });

                var videoUploadQueueName = configuration.GetValue<string>("VideoUploadedEventConsumerQueue")?.ToString();
                if (!string.IsNullOrEmpty(videoUploadQueueName))
                {
                    cfg.ReceiveEndpoint(videoUploadQueueName, con =>
                    {
                        con.Consumer<VideoUploadedEventConsumer>(context);
                    });
                }
            });
        });

        #endregion

        services.Configure<MongoDbSetting>(configuration.GetSection(nameof(MongoDbSetting)));
        services.AddSingleton<IMongoDbContext, MongoDbContext>();

        return services;
    }
}
