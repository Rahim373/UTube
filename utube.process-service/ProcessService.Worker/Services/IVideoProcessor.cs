namespace ProcessService.Worker.Services
{
    public interface IVideoProcessor
    {
        Task ProcessThumbnailAsync(string videoId, string videoPath, CancellationToken cancellationToken = default);
        Task ResizeVideoAsync(string videoId, string videoPath, CancellationToken cancellationToken = default);
    }
}
