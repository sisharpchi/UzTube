namespace Application.Dtos.Subscription;

public class SubscriptionListItemDto
{
    public long ChannelId { get; set; }
    public string ChannelName { get; set; }
    public int ChannelSubscriberCount { get; set; }
}