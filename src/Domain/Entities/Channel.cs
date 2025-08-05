namespace Domain.Entities;

public class Channel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public string? AvatarUrl { get; set; } // cloud storage url
    public string? AvatarCloudPublicId { get; set; } // for deleting/editing in cloud storage

    public string? BannerUrl { get; set; } // cloud storage url
    public string? BannerCloudPublicId { get; set; } // for deleting/editing in cloud storage

    public long OwnerId { get; set; }

    public ICollection<Video>? Videos { get; set; }
    public ICollection<Subscription>? Subscribers { get; set; } 
    public ICollection<Playlist>? Playlists { get; set; }
}
