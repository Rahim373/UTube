using Grpc.Core;
using Mapster;
using MediatR;
using VideoService.Application.Commands;
using VideoService.Application.Dtos;
using VideoService.Application.Protos;
using static VideoService.Application.Protos.GrpcVideoService;

namespace VideoService.Application.GRpc
{
    public class GrpcVideoService : GrpcVideoServiceBase
    {
        private readonly ISender _sender;

        public GrpcVideoService(ISender sender)
        {
            _sender = sender;
        }

        public override async Task<UpdateVideoResponse> UpdateVideo(UpdateVideoRequest request, ServerCallContext context)
        {
            var success = await _sender.Send(new UpdateVideoFieldsCommand(request.VideoId, request.UpdateDetails.Adapt<List<UpdateDto>>()));
            return await Task.FromResult(new UpdateVideoResponse { Success = success });
        }
    }
}
