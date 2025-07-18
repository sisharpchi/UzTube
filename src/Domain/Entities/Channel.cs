﻿namespace Domain.Entities;

public class Channel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public long OwnerId { get; set; }

    public ICollection<Video> Videos { get; set; }
    public ICollection<Subscription> Subscribers { get; set; } 
    public ICollection<Playlist> Playlists { get; set; }
}
