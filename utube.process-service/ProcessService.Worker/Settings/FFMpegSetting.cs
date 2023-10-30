namespace ProcessService.Worker.Settings;

public class FFMpegSetting
{
    public string ExecutablePath { get; set; } = string.Empty;
    public string FFMpegExeutableName { get; set; } = "ffmpeg";
    public string FFProbeExecutableName {get; set; } = "ffprobe";
}
