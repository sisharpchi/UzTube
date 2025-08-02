using Application.Contracts.Repository;
using Application.Contracts.Sevice;
using Application.Dtos.Channel;
using Application.Dtos.Subscription;
using Domain.Entities;

namespace Application.Services;

public class SubscriptionService(ISubscriptionRepository subscriptionRepository) : ISubscriptionService
{
    public async Task<int> GetSubscriberCountAsync(long channelId)
    {
        var channelSubscriberCount = await subscriptionRepository.CountSubscribersAsync(channelId);
        return channelSubscriberCount;
    }

    public async Task<List<ChannelListItemDto>> GetUserSubscriptionsAsync(long userId)
    {
        var subscriptions = await subscriptionRepository.GetByUserIdAsync(userId);
        return subscriptions.Select(s => ConvertToChannelListItemDto(s)).ToList();
    }

    private ChannelListItemDto ConvertToChannelListItemDto(Subscription s)
    {
        return new ChannelListItemDto
        {
            Id = s.ChannelId,
            ChannelDto = ConvertChannelToDto(s.Channel)
        };
    }

    public async Task<bool> IsSubscribedAsync(long subscriberId, long channelId)
    {
        var subscription = await subscriptionRepository.GetByUserAndChannelIdAsync(subscriberId, channelId);
        if (subscription is null) return false;
        return true;
    }

    public async Task<bool> ToggleSubscriptionAsync(long userId, SubscriptionCreateDto subscriptionCreateDto)
    {
        var isSubscribed = await IsSubscribedAsync(userId, subscriptionCreateDto.ChannelId);
        if (isSubscribed)
        {
            await subscriptionRepository.DeleteAsync(userId, subscriptionCreateDto.ChannelId);
            return false;
        }
        else
        {
            subscriptionCreateDto = new SubscriptionCreateDto
            {
                ChannelId = subscriptionCreateDto.ChannelId
            };
            var subscription = ConvertToSubscription(subscriptionCreateDto);
            subscription.SubscriberId = userId;
            await subscriptionRepository.AddAsync(subscription);
            return true;
        }
    }

    private Subscription ConvertToSubscription(SubscriptionCreateDto subscriptionCreateDto)
    {
        return new Subscription
        {
            ChannelId = subscriptionCreateDto.ChannelId,
        };
    }

    private ChannelDto ConvertChannelToDto(Channel channel)
    {
        return new ChannelDto()
        {
            Id = channel.Id,
            Description = channel.Description,
            Name = channel.Name,
            OwnerId = channel.OwnerId,
            PlaylistCount = channel.Playlists?.Count ?? 0,
            SubscriberCount = channel.Subscribers?.Count ?? 0,
            VideoCount = channel.Videos?.Count ?? 0,
        };
    }
}
