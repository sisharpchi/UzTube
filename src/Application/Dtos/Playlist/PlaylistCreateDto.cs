namespace Application.Dtos.Playlist;

public class PlaylistCreateDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public long ChannelId { get; set; }
}