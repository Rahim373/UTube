using MassTransit;
using MassTransit.Transports.Fabric;
using ProcessService.Worker.Consumers;
using ProcessService.Worker.Protos;
using ProcessService.Worker.Services;
using ProcessService.Worker.Settings;
using UTube.Common.DependencyInjection;
using UTube.Common.Events;
using Xabe.FFmpeg;

namespace ProcessService.Worker;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, ConfigurationManager configuration)
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

                cfg.ReceiveEndpoint(configuration.GetValue<string>("VideoUploadedEventConsumerQueue").ToString(), con =>
                {
                    con.Consumer<VideoUploadedEventConsumer>(context);
                });
            });
        });

        #endregion

        #region gRPC

        var grpcFileServiceHost = configuration.GetSection("GrpcClients:StorageService").Value;

        if (!string.IsNullOrEmpty(grpcFileServiceHost))
        {
            services.AddGrpcClient<GrpcFileService.GrpcFileServiceClient>(option =>
            {
                option.Address = new Uri(grpcFileServiceHost);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                return handler;
            });
        }

        var grpcVideoServiceHost = configuration.GetSection("GrpcClients:VideoService").Value;

        if (!string.IsNullOrEmpty(grpcVideoServiceHost))
        {
            services.AddGrpcClient<GrpcVideoService.GrpcVideoServiceClient>(option =>
            {
                option.Address = new Uri(grpcVideoServiceHost);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                return handler;
            });
        }

        #endregion

        var ffMpegSetting = new FFMpegSetting();
        configuration.GetSection(nameof(FFMpegSetting)).Bind(ffMpegSetting);

        services.AddTransient<IFFMpegService, FFMpegService>();
        services.AddTransient<IFileService, FileService>();

        FFmpeg.SetExecutablesPath(ffMpegSetting.ExecutablePath, ffMpegSetting.FFMpegExeutableName, ffMpegSetting.FFProbeExecutableName);

        services.AddSerilog(configuration);
        services.AddOpenTelemetry(configuration);

        services.AddTransient<IVideoProcessor, VideoProcessor>();

        return services;
    }
}
