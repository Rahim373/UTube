namespace UTube.Common.Events;

public record VideoUploadedEvent (string videoId, string objectPath, string objectUrl);