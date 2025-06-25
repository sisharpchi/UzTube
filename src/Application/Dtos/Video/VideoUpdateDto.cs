namespace Application.Dtos.Video;

public class VideoUpdateDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public long? PlaylistId { get; set; }
    public List<long>? TagIds { get; set; }
}