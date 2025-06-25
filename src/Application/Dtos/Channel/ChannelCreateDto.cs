namespace Application.Dtos.Channel;

public class ChannelCreateDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public long OwnerId { get; set; }
}