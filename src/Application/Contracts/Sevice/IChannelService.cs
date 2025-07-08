using Application.Dtos.Channel;

namespace Application.Contracts.Sevice;

public interface IChannelService
{
    // 1. Create a new channel
    Task<ChannelDto> CreateAsync(ChannelCreateDto dto);

    // 2. Get channel by userId (har bir userda 1 ta kanal)
    Task<ChannelDto> GetByUserIdAsync(long userId);

    // 3. Get channel with video list
    Task<ChannelWithVideosDto> GetWithVideosAsync(long channelId);

    // 4. Update channel info
    Task<ChannelDto> UpdateAsync(long channelId, ChannelUpdateDto dto);

    // 5. Get subscriber count
    Task<int> GetSubscriberCountAsync(long channelId);

    // 6. Get channels by filter/search query (e.g. name)
    Task<List<ChannelListItemDto>> SearchAsync(string? searchQuery);
}
