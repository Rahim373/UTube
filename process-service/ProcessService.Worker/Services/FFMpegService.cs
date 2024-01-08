using ProcessService.Worker.Enums;
using ProcessService.Worker.Protos;
using System.Text;
using Xabe.FFmpeg;

namespace ProcessService.Worker.Services;

public class FFMpegService : IFFMpegService
{
    /// <summary>
    /// Creates thumbnail from a video
    /// </summary>
    /// <param name="videoPath">Video path</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Thumbnail path, seconds to complete the task</returns>
    public async Task<(string thumbnailPath, long timeTaken)> GenerateThumbnailAsync(string videoPath, CancellationToken cancellationToken = default)
    {
        var outputPath = GetFilePathFromTempDirectory($"{Guid.NewGuid()}.png");
        var arguments = $@"-i {videoPath} -vf ""thumbnail=300"" -frames:v 1 {outputPath}";
        var conversion = await FFmpeg.Conversions.New().Start(arguments, cancellationToken);
        return await Task.FromResult((outputPath, conversion.Duration.Seconds));
    }

    /// <summary>
    /// Gets dimension and duration of a video
    /// </summary>
    /// <param name="videoFilePath">Video path</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(int Width, int Height, TimeSpan Duration)> GetVideoDimensionAndDurationAsync(string videoFilePath, CancellationToken cancellationToken = default)
    {
        var mediaInfo = await FFmpeg.GetMediaInfo(videoFilePath, cancellationToken);

        if (mediaInfo is not null)
        {
            var videoStream = mediaInfo.VideoStreams.FirstOrDefault();

            if (videoStream is not null)
            {
                return await Task.FromResult((videoStream.Width, videoStream.Height, videoStream.Duration));
            }
        }

        return (0, 0, new TimeSpan(0));
    }

    /// <summary>
    /// Changes the dimention of a video maintaining the aspect ratio
    /// </summary>
    /// <param name="videoFilePath">Video path</param>
    /// <param name="targetWidth">target width</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Outout file path, seconds to compelete the task</returns>
    public async Task<(string? outputDirectory, long timeTaken)> ScaleVideoAndConvertToM3U8Async(string videoId, string videoFilePath, List<VideoWidth> types, CancellationToken cancellationToken = default)
    {
        var (outputDirectory, outputFilePath) = GenerateDirectoryAndPathForM3U8(videoId);
        var dimentions = await GetHeightWidthAsync(types, videoFilePath);

        try
        {
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            // var args = GetFfmpegArgs(videoFilePath, dimentions, outputDirectory);
            var args = $@"-i {videoFilePath} -hls_time 10 -hls_list_size 0 -hls_flags delete_segments -hls_segment_filename {outputDirectory}/{videoId}_%03d.ts {outputFilePath}";

            var conversion = await FFmpeg.Conversions.New().UseMultiThread(true).Start(args);

            return (outputDirectory, conversion.Duration.Seconds);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Directory.Delete(outputDirectory, true);
        }

        return (null, 0);
    }

    private string GetFfmpegArgs(string videoFilePath, List<(int width, int height)> dimentions, string outputDirectory)
    {
        var count = dimentions.Count;

        var filter_complex = new StringBuilder($"[0:v]split={count}");
        var filterComplexPart = new List<string>();
        var mapVideoPart = new List<string>();
        var mapAudioPart = new List<string>();
        var streamMap = new List<string>();

        for (int i = 1; i <= count; i++)
        {
            filter_complex.Append($"[v{i}]");

            if (i == 1)
            {
                filterComplexPart.Add($"[v{i}]copy[v{i}out]");
            }
            else
            {
                filterComplexPart.Add($"[v{i}]scale=w={dimentions[i - 1].width}:h={dimentions[i - 1].height}[v{i}out]");
            }

            var bit = i == 1 ? 5 : i == 2 ? 3 : 1;

            mapVideoPart.Add($@"-map ""[v{i}out]"" -c:v:{i - 1} libx264 -x264-params ""nal-hrd=cbr:force-cfr=1"" -b:v:{i - 1} {bit}M -maxrate:v:{i - 1} {bit}M -minrate:v:{i - 1} {bit}M -bufsize:v:{i - 1} {bit}M -preset slow -g 48 -sc_threshold 0 -keyint_min 48");
            mapAudioPart.Add($@"-map a:0 -c:a:{i - 1} aac -b:a:{i - 1} 96k -ac 2");
            streamMap.Add($"v:{i - 1},a:{i - 1}");
        }

        filter_complex.Append("; ");
        filter_complex.Append(string.Join("; ", filterComplexPart));

        var videoMaps = string.Join(" ", mapVideoPart);
        var audioMaps = string.Join(" ", mapAudioPart);
        var streamMaps = string.Join(" ", streamMap);


        var args = $"ffmpeg -i {videoFilePath} -filter_complex \"{filter_complex}\" {videoMaps} {audioMaps} -f hls -hls_time 2 -hls_playlist_type vod -hls_flags independent_segments -hls_segment_type mpegts -hls_segment_filename {outputDirectory}/data_%v_%06d.ts -master_pl_name {outputDirectory}/master.m3u8 -var_stream_map \"{streamMaps}\" {outputDirectory}/stream_%v.m3u8";

        return args;
    }

    private string GetFilePathFromTempDirectory(string filename)
    {
        return Path.Combine(Path.GetTempPath(), filename);
    }

    private (string outputDirectory, string outputFilePath) GenerateDirectoryAndPathForM3U8(string videoId)
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var outputFilePath = Path.Combine(directory, $"{videoId}.m3u8");
        return (directory, outputFilePath);
    }

    private async Task<List<(int width, int height)>> GetHeightWidthAsync(List<VideoWidth> types, string inputFilePath)
    {
        var dimensions = new List<(int width, int height)>();
        foreach (var type in types)
        {
            var mediaInfo = await FFmpeg.GetMediaInfo(inputFilePath);
            var videoStream = mediaInfo.VideoStreams.FirstOrDefault();

            if (videoStream is null) dimensions.Add((0, 0));

            int originalWidth = videoStream.Width;
            int originalHeight = videoStream.Height;

            int expectedHeight = type switch
            {
                VideoWidth.Hd720 => 720,
                VideoWidth.Sd480 => 480,
                _ => 1080
            };

            double aspectRatio = (double)originalWidth / originalHeight;
            int calculatedWidth = (int)Math.Round(expectedHeight * aspectRatio);
            calculatedWidth = calculatedWidth % 2 != 0 
                ? calculatedWidth + 1 : calculatedWidth;
            dimensions.Add((calculatedWidth, expectedHeight));
        }

        return dimensions;
    }
}
