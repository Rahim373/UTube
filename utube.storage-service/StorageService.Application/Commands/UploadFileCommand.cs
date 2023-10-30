using MassTransit;
using MediatR;
using MimeTypes;
using StorageService.Application.Dto;
using StorageService.Application.Enums;
using StorageService.Application.Protos;
using StorageService.Application.Services;
using StorageService.Application.Setting;
using UTube.Common.Events;
using static StorageService.Application.Protos.GrpcVideoService;

namespace StorageService.Application.Commands;

public record UploadFileCommand(
    Stream stream,
    UploadTypes types,
    string mimeType,
    string videoId,
    string? fileName = null) : IRequest<UploadFileResponse>;


public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, UploadFileResponse>
{
    private const string MASTER_PLAYLIST_FILENAME = "master.m3u8";
    private readonly IBus _bus;
    private readonly IFileService _fileService;
    private readonly StorageSetting _storageSetting;
    private readonly GrpcVideoServiceClient _grpcVideoServiceClient;

    public UploadFileCommandHandler(IFileService fileService, IBus bus,
        StorageSetting storageSetting, GrpcVideoServiceClient grpcVideoServiceClient)
    {
        _fileService = fileService;
        _bus = bus;
        _storageSetting = storageSetting;
        _grpcVideoServiceClient = grpcVideoServiceClient;
    }


    public async Task<UploadFileResponse> Handle(UploadFileCommand command, CancellationToken cancellationToken)
    {
        var videoId = command.types == UploadTypes.VIDEO ?
            Guid.NewGuid().ToString().ToLower() : command.videoId;

        string fileName = GenerateFileName(videoId, command.types, command.fileName, command.mimeType);

        var objectPath = await _fileService.UploadFileAsync(videoId, command.stream, fileName, command.mimeType, cancellationToken);
        var objectUrl = _storageSetting.GetObjectUrl(objectPath);

        if (command.types == UploadTypes.VIDEO)
        {
            await _bus.Publish(
                new VideoUploadedEvent(videoId, objectPath, objectUrl)).ConfigureAwait(false);
        }

        if (command.types is UploadTypes.THUMBNAIL || 
            (command.types == UploadTypes.PLAYLIST && Path.GetFileName(fileName).ToLower().EndsWith("m3u8")))
        {
            var updateData = new UpdateVideoRequest
            {
                VideoId = videoId
            };
            updateData.UpdateDetails.Add(new UpdateDetail
            {
                Field = command.types is UploadTypes.THUMBNAIL ? FieldType.Thumbnail : FieldType.Playlist,
                Value = objectPath
            });

            await _grpcVideoServiceClient.UpdateVideoAsync(updateData, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        command.stream.Dispose();
        var response = new UploadFileResponse(videoId, objectPath, _storageSetting.GetObjectUrl(objectPath));
        return await Task.FromResult(response);
    }

    private string GenerateFileName(string videoId, UploadTypes types, string? fileName, string mimeType)
    {
        if (types == UploadTypes.THUMBNAIL)
        {
            fileName = Guid.NewGuid() + MimeTypeMap.GetExtension(mimeType);
        }
        else
        {
            fileName = string.IsNullOrEmpty(fileName) ? videoId + MimeTypeMap.GetExtension(mimeType) : fileName;
        }

        return $"{videoId}/" + types switch
        {
            UploadTypes.VIDEO => $"",
            UploadTypes.THUMBNAIL => $"thumbnail/",
            UploadTypes.PLAYLIST => $"playlist/",
            _ => $"temp/"
        } + fileName;
    }
}
