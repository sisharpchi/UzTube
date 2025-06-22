namespace Domain.Entities;

public class Subscription
{
    public long Id { get; set; }

    public long SubscriberId { get; set; }
    public User Subscriber { get; set; } = null!;

    public long ChannelId { get; set; }
    public Channel Channel { get; set; } = null!;
}
