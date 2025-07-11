using Application.Dtos.Channel;
using Application.Dtos.Subscription;

namespace Application.Contracts.Sevice;

public interface ISubscriptionService
{
    Task<long> ToggleSubscriptionAsync(long userId, SubscriptionCreateDto subscriptionCreateDto);
    Task<bool> IsSubscribedAsync(long subscriberId, long channelId);
    Task<int> GetSubscriberCountAsync(long channelId);
    Task<List<ChannelListItemDto>> GetUserSubscriptionsAsync(long userId);
}