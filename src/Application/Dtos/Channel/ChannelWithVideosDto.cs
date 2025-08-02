using Application.Dtos.Video;

namespace Application.Dtos.Channel;

public class ChannelWithVideosDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public string? AvatarUrl { get; set; } // cloud storage url
    public string? AvatarCloudPublicId { get; set; } // for deleting/editing in cloud storage

    public string? BannerUrl { get; set; } // cloud storage url
    public string? BannerCloudPublicId { get; set; } // for deleting/editing in cloud storage


    public List<VideoDto> Videos { get; set; }
}