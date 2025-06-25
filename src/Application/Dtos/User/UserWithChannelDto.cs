using Application.Dtos.Channel;

namespace Application.Dtos.User;

public class UserWithChannelDto
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? ProfileImageUrl { get; set; }

    public ChannelDto Channel { get; set; }
}