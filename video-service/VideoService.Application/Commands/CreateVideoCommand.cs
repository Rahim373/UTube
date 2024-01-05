using MediatR;
using MongoDB.Driver;
using VideoService.Application.Context;
using VideoService.Domain.Models;

namespace VideoService.Application.Commands;

public record CreateVideoCommand(string VideoId, string VideoPath) : IRequest { }

public class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand>
{
    private readonly IMongoCollection<Video> _videoCollection;

    public CreateVideoCommandHandler(IMongoDbContext databaseContext)
    {
        _videoCollection = databaseContext.GetCollection<Video>();
    }

    public async Task Handle(CreateVideoCommand request, CancellationToken cancellationToken)
    {
        await _videoCollection.InsertOneAsync(new Video
        {
            VideoId = request.VideoId,
            Title = "Untitled",
            UploadedOn = DateTime.UtcNow,
        });
    }
}
