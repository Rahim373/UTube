using MassTransit;
using MediatR;
using UTube.Common.Events;
using VideoService.Application.Commands;

namespace VideoService.Application.Consumers;

public class VideoUploadedEventConsumer : IConsumer<VideoUploadedEvent>
{
    private readonly IMediator _mediator;

    public VideoUploadedEventConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<VideoUploadedEvent> context)
    {
        await _mediator.Send(new CreateVideoCommand(context.Message.videoId, context.Message.objectPath), context.CancellationToken);
    }
}