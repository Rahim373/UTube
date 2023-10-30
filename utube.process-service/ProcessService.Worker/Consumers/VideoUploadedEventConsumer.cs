using MassTransit;
using ProcessService.Worker.Services;
using System.Diagnostics;
using UTube.Common.Events;

namespace ProcessService.Worker.Consumers;

public class VideoUploadedEventConsumer : IConsumer<VideoUploadedEvent>
{
    private readonly IVideoProcessor _videoProcessor;
    private readonly ILogger<VideoUploadedEventConsumer> _logger;

    public VideoUploadedEventConsumer(ILogger<VideoUploadedEventConsumer> logger,
       IVideoProcessor videoProcessor)
    {
        _logger = logger;
        _videoProcessor = videoProcessor;
    }

    public async Task Consume(ConsumeContext<VideoUploadedEvent> context)
    {
        try
        {
            _logger.LogInformation($"Event received on {nameof(VideoUploadedEventConsumer)}");

            // var videoProcessor = context.GetServiceOrCreateInstance<IVideoProcessor>();
            var thumbnailTask = _videoProcessor.ProcessThumbnailAsync(context.Message.videoId, context.Message.objectUrl, context.CancellationToken);
            var resizeTask = _videoProcessor.ResizeVideoAsync(context.Message.videoId, context.Message.objectUrl, context.CancellationToken);
            await Task.WhenAll(thumbnailTask, resizeTask);

            Debug.WriteLine(context.Message.videoId);

            _logger.LogInformation($"Event completed from {nameof(VideoUploadedEventConsumer)}");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
