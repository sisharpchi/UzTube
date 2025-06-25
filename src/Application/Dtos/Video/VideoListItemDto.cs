namespace Application.Dtos.Video;

public class VideoListItemDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string? ThumbnailUrl { get; set; }
    public TimeSpan Duration { get; set; }

    public string ChannelName { get; set; }
    public DateTime UploadedAt { get; set; }
    public int ViewCount { get; set; }
}