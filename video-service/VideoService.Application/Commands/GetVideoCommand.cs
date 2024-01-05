using Mapster;
using MediatR;
using MongoDB.Driver;
using VideoService.Application.Context;
using VideoService.Application.Dtos;
using VideoService.Domain.Models;

namespace VideoService.Application.Commands
{
    public record GetVideoCommand(Guid VideoId) : IRequest<VideoDto>
    {
    }

    public class GetVideoCommandHandler : IRequestHandler<GetVideoCommand, VideoDto>
    {
        private readonly IMongoCollection<Video> _videoCollection;

        public GetVideoCommandHandler(IMongoDbContext dbContext)
        {
            _videoCollection = dbContext.GetCollection<Video>();
        }

        public async Task<VideoDto> Handle(GetVideoCommand request, CancellationToken cancellationToken)
        {
            var video = await _videoCollection
                .Find(Builders<Video>.Filter.Eq(v => v.VideoId, request.VideoId.ToString()))
                .FirstOrDefaultAsync(cancellationToken);

            return await Task.FromResult(video.Adapt<VideoDto>());
        }
    }
}
