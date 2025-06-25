namespace Application.Dtos.Video;

public class VideoUploadDto
{
    public string Title { get; set; }
    public string? Description { get; set; }

    public string VideoUrl { get; set; } // Cloud URL yoki local path
    public string? ThumbnailUrl { get; set; }
    public string? CloudPublicId { get; set; }

    public TimeSpan Duration { get; set; }
    public long ChannelId { get; set; }
    public long? PlaylistId { get; set; }

    public List<long>? TagIds { get; set; } // agar taglar biriktirilsa
}
