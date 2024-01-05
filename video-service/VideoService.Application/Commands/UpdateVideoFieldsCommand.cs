using MediatR;
using MongoDB.Driver;
using VideoService.Application.Context;
using VideoService.Application.Dtos;
using VideoService.Domain.Models;

namespace VideoService.Application.Commands;

public record UpdateVideoFieldsCommand(string VideoId, IList<UpdateDto> UpdateDetails) : IRequest<bool> { }

public class UpdateVideoFieldsCommandHandler : IRequestHandler<UpdateVideoFieldsCommand, bool>
{
    private readonly IMongoCollection<Video> _videoCollection;

    public UpdateVideoFieldsCommandHandler(IMongoDbContext databaseContext)
    {
        _videoCollection = databaseContext.GetCollection<Video>();
    }

    public async Task<bool> Handle(UpdateVideoFieldsCommand request, CancellationToken cancellationToken)
    {
        var filter = Builders<Video>.Filter.Eq(v => v.VideoId, request.VideoId);

        var update = Builders<Video>.Update;
        var updates = new List<UpdateDefinition<Video>>();

        foreach (var updateDetail in request.UpdateDetails)
        {
            switch (updateDetail.Field)
            {
                case FieldType.Playlist:
                    updates.Add(update.Set(x => x.Playlist, updateDetail.Value));
                    break;
                case FieldType.Duration:
                    if (double.TryParse(updateDetail.Value, out double seconds))
                    {
                        var time = TimeSpan.FromSeconds(seconds);
                        string duration = time.ToString(@"hh\:mm\:ss");
                        updates.Add(update.Set(x => x.Duration, duration));
                    }
                    break;
                case FieldType.Title:
                    updates.Add(update.Set(x => x.Title, updateDetail.Value));
                    break;
                case FieldType.Thumbnail:
                    updates.Add(update.Set(x => x.DefaultThumbnail, updateDetail.Value));
                    break;
                default:
                    break;
            }
        }

        var updateResult = await _videoCollection.UpdateOneAsync(filter, update.Combine(updates));
        return await Task.FromResult(updateResult.IsAcknowledged && updateResult.ModifiedCount > 0);
    }
}
