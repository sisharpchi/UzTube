namespace Application.Dtos.Channel;

public class ChannelDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public long OwnerId { get; set; }
    public string OwnerUsername { get; set; } // yoki OwnerDto ichida ko‘rsatish mumkin

    public int VideoCount { get; set; }
    public int SubscriberCount { get; set; }
    public int PlaylistCount { get; set; }
}