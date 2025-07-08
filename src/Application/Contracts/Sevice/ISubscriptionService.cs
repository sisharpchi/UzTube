using Application.Dtos.Channel;

namespace Application.Contracts.Sevice;

public interface ISubscriptionService
{
    Task<bool> ToggleSubscriptionAsync(long subscriberId, long channelId);
    Task<bool> IsSubscribedAsync(long subscriberId, long channelId);
    Task<int> GetSubscriberCountAsync(long channelId);
    Task<List<ChannelListItemDto>> GetUserSubscriptionsAsync(long userId);
}