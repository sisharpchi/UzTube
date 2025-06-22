namespace Domain.Entities;

public class Channel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public long OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public ICollection<Video> Videos { get; set; }
    public ICollection<Subscription> Subscribers { get; set; } 
    public ICollection<Playlist> Playlists { get; set; }
}
