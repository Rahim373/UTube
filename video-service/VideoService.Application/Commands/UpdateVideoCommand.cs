using MediatR;
using MongoDB.Driver;
using VideoService.Application.Context;
using VideoService.Domain.Models;

namespace VideoService.Application.Commands;

public record UpdateVideoCommand(string VideoId, string Title) : IRequest<bool> { }

public class UpdateVideoCommandHandler : IRequestHandler<UpdateVideoCommand, bool>
{
    private readonly IMongoCollection<Video> _videoCollection;

    public UpdateVideoCommandHandler(IMongoDbContext databaseContext)
    {
        _videoCollection = databaseContext.GetCollection<Video>();
    }

    public async Task<bool> Handle(UpdateVideoCommand request, CancellationToken cancellationToken)
    {
        var updateResult = await _videoCollection.UpdateOneAsync(
            rec => rec.VideoId == Guid.Parse(request.VideoId),
            Builders<Video>.Update
                .Set(rec => rec.Title, request.Title)
            ).ConfigureAwait(false);

        return await Task.FromResult(updateResult.IsAcknowledged &&
        updateResult.ModifiedCount > 0);
    }
}
