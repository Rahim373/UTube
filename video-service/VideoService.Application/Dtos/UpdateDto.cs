namespace VideoService.Application.Dtos
{
    public enum FieldType
    {
        Title = 0,
        Playlist = 1,
        Thumbnail = 2,
        Duration = 3,
    }

    public record UpdateDto(FieldType Field, string Value) { }
}
