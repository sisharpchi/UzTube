namespace Domain.Entities;

public class Subscription
{
    public long Id { get; set; }

    public long SubscriberId { get; set; }

    public long ChannelId { get; set; }
    public Channel Channel { get; set; }
    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
}
