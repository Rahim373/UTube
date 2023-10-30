using MimeTypes;
using ProcessService.Worker.Enums;
using ProcessService.Worker.Protos;
using static ProcessService.Worker.Protos.GrpcFileService;
using static ProcessService.Worker.Protos.GrpcVideoService;

namespace ProcessService.Worker.Services
{
    public class VideoProcessor : IVideoProcessor
    {
        private readonly GrpcFileServiceClient _grpcFileServiceClient;
        private readonly GrpcVideoServiceClient _grpcVideoServiceClient;
        private readonly IFFMpegService _ffMpegService;
        private readonly IFileService _fileService;

        public VideoProcessor(
            GrpcFileServiceClient grpcFileServiceClient,
            GrpcVideoServiceClient grpcVideoServiceClient,
            IFFMpegService ffMpegService,
            IFileService fileService)
        {
            _grpcFileServiceClient = grpcFileServiceClient;
            _grpcVideoServiceClient = grpcVideoServiceClient;
            _ffMpegService = ffMpegService;
            _fileService = fileService;
        }

        public async Task ProcessThumbnailAsync(string videoId, string videoPath, CancellationToken cancellationToken = default)
        {
            var (thumbnailPath, timetaken) = await _ffMpegService.GenerateThumbnailAsync(videoPath);

            if (!string.IsNullOrWhiteSpace(thumbnailPath) && File.Exists(thumbnailPath))
            {
                var uploadStream = _grpcFileServiceClient.UploadFile();

                await _fileService.PrepareFileStreamAsync(thumbnailPath, (data) =>
                {
                    uploadStream.RequestStream.WriteAsync(new UploadFileRequest()
                    {
                        Chunk = new DataChunk { Data = data },
                        VideoId = videoId,
                        MimeType = MimeTypeMap.GetMimeType(thumbnailPath),
                        Type = UploadFileType.Thumbnail
                    }, cancellationToken).Wait(cancellationToken);
                }, cancellationToken);

                await uploadStream.RequestStream.CompleteAsync();

                File.Delete(thumbnailPath);
            }

            await Task.CompletedTask;
        }

        public async Task ResizeVideoAsync(string videoId, string videoPath, CancellationToken cancellationToken = default)
        {
            var data = await _ffMpegService.GetVideoDimensionAndDurationAsync(videoPath, cancellationToken);

            var durationData = new UpdateVideoRequest
            {
                VideoId = videoId
            };
            durationData.UpdateDetails.Add(new UpdateDetail
            {
                Field = FieldType.Duration,
                Value = data.Duration.TotalSeconds.ToString()
            });
            await _grpcVideoServiceClient.UpdateVideoAsync(durationData, cancellationToken: cancellationToken);

            var variations = GetVariations(data.Width);
            await DownscaleAndUpload(videoId, videoPath, variations, cancellationToken);
        }

        private async Task DownscaleAndUpload(string videoId, string videoPath, List<VideoWidth> types, CancellationToken cancellationToken)
        {
            var (outputDirectory, time) = await _ffMpegService.ScaleVideoAndConvertToM3U8Async(videoId, videoPath, types, cancellationToken);

            if (!string.IsNullOrEmpty(outputDirectory) && Directory.Exists(outputDirectory))
            {
                var files = Directory.GetFiles(outputDirectory);

                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = 4
                };

                await Parallel.ForEachAsync(files, parallelOptions, async (file, token) =>
                {
                    var uploadStream = _grpcFileServiceClient.UploadFile();
                    var mimeType = MimeTypeMap.GetMimeType(file);

                    await _fileService.PrepareFileStreamAsync(file, (data) =>
                    {
                        uploadStream.RequestStream.WriteAsync(new UploadFileRequest()
                        {
                            Chunk = new DataChunk { Data = data },
                            VideoId = videoId,
                            MimeType = mimeType,
                            Type = UploadFileType.Playlist,
                            FileName = Path.GetFileName(file),
                        }, cancellationToken).Wait(cancellationToken);
                    }, cancellationToken);

                    await uploadStream.RequestStream.CompleteAsync();
                });

                Directory.Delete(outputDirectory, true);
            }
        }

        private static List<VideoWidth> GetVariations(int width)
        {
            var variations = new List<VideoWidth>();

            if (width >= 1080)
                variations.Add(VideoWidth.Fhd1080);

            if (width >= 720)
                variations.Add(VideoWidth.Hd720);

            if (width >= 480)
                variations.Add(VideoWidth.Sd480);

            return variations;
        }
    }
}
