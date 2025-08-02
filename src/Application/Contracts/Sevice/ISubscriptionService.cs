using Application.Dtos.Channel;
using Application.Dtos.Subscription;

namespace Application.Contracts.Sevice;

public interface ISubscriptionService
{
    Task<bool> ToggleSubscriptionAsync(long userId, SubscriptionCreateDto subscriptionCreateDto);
    Task<bool> IsSubscribedAsync(long subscriberId, long channelId);
    Task<int> GetSubscriberCountAsync(long channelId);
    Task<List<ChannelListItemDto>> GetUserSubscriptionsAsync(long userId);
}