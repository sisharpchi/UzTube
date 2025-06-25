namespace Application.Dtos.Subscription;

public class SubscriptionDto
{
    public long Id { get; set; }

    public long SubscriberId { get; set; }
    public string SubscriberUsername { get; set; }

    public long ChannelId { get; set; }
    public string ChannelName { get; set; }

    public DateTime SubscribedAt { get; set; } // Agar entitiyga qo‘shilsa
}