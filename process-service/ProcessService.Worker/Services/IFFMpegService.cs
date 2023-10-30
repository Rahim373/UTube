using ProcessService.Worker.Enums;
using ProcessService.Worker.Protos;

namespace ProcessService.Worker.Services;

public interface IFFMpegService
{
    Task<(string thumbnailPath, long timeTaken)> GenerateThumbnailAsync(string videoPath, CancellationToken cancellationToken = default);
    Task<(int Width, int Height, TimeSpan Duration)> GetVideoDimensionAndDurationAsync(string videoFilePath, CancellationToken cancellationToken = default);
    Task<(string outputDirectory, long timeTaken)> ScaleVideoAndConvertToM3U8Async(string videoId, string videoFilePath, List<VideoWidth> type, CancellationToken cancellationToken = default);
}
