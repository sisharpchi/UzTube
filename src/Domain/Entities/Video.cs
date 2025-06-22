using System.Xml.Linq;

namespace Domain.Entities;

public class Video
{
    public long Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string VideoUrl { get; set; } = null!;
    public string? ThumbnailUrl { get; set; } // cloud storage url
    public string? CloudPublicId { get; set; } // for deleting/editing in cloud storage
    public TimeSpan Duration { get; set; }
    public DateTime UploadedAt { get; set; }

    public long ChannelId { get; set; }
    public Channel Channel { get; set; } = null!;

    public long? PlaylistId { get; set; }
    public Playlist? Playlist { get; set; }

    public ICollection<Comment> Comments { get; set; } 
    public ICollection<LikeDislike> Likes { get; set; } 
    public ICollection<ViewHistory> ViewHistories { get; set; } 
    public ICollection<VideoTag> VideoTags { get; set; } 
    public ICollection<VideoReport> Reports { get; set; } 
}
