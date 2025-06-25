namespace Application.Dtos.Playlist;

public class PlaylistDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public long ChannelId { get; set; }
    public string ChannelName { get; set; } // optional: ko‘rsatish uchun

    public List<VideoDto> Videos { get; set; }
}