using Application.Dtos.Channel;
using Application.Dtos.Tag;

namespace Application.Dtos.Video;

public class VideoDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public string VideoUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime UploadedAt { get; set; }

    public long ChannelId { get; set; }
    public ChannelDto? Channel { get; set; }
    public bool IsSubscribed { get; set; }
    public long? PlaylistId { get; set; }
    public string? PlaylistName { get; set; }

    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
    public int ViewCount { get; set; }

    public List<TagDto> Tags { get; set; }
}