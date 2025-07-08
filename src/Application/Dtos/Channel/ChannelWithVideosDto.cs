using Application.Dtos.Video;

namespace Application.Dtos.Channel;

public class ChannelWithVideosDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public List<VideoDto> Videos { get; set; }
}