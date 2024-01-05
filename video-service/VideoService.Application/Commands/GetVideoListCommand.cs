using Mapster;
using MediatR;
using MongoDB.Driver;
using VideoService.Application.Context;
using VideoService.Application.Dtos;
using VideoService.Domain.Models;

namespace VideoService.Application.Commands
{
    public record GetVideoListCommand(int? pageLength, int? pageNumber) : IRequest<List<VideoDto>>
    {
    }

    public class GetVideoListCommandHandler : IRequestHandler<GetVideoListCommand, List<VideoDto>>
    {
        private readonly IMongoCollection<Video> _videoCollection;

        public GetVideoListCommandHandler(IMongoDbContext dbContext)
        {
            _videoCollection = dbContext.GetCollection<Video>();
        }

        public async Task<List<VideoDto>> Handle(GetVideoListCommand request, CancellationToken cancellationToken)
        {
            var take = request.pageLength is < 1 or null ? 50 : request.pageLength;
            var skip = (request.pageNumber is < 1 or null ? 1 : request.pageNumber) - 1;

            var videos = await _videoCollection
                .Find(Builders<Video>.Filter.Empty)
                .Skip(skip * take)
                .Limit(take)
                .ToListAsync(cancellationToken);

            return await Task.FromResult(videos.Adapt<List<VideoDto>>());
        }
    }
}
