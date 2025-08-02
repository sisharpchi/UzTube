using Application.Dtos.Channel;

namespace Application.Contracts.Sevice;

public interface IChannelService
{
    // 1. Create a new channel
    Task<ChannelWithVideosDto> CreateAsync(long userId, ChannelCreateDto dto);

    // 2. Get channel by userId (har bir userda 1 ta kanal)
    Task<ChannelWithVideosDto> GetByUserIdAsync(long userId);

    // 3. Get channel with video list
    Task<ChannelWithVideosDto> GetWithVideosAsync(long userId);

    // 4. Update channel info
    Task UpdateAsync(long userId, ChannelUpdateDto dto);

    // 5. Get subscriber count
    Task<int> GetSubscriberCountAsync(long userId);

    // 6. Get channels by filter/search query (e.g. name)
    List<ChannelListItemDto> SearchAsync(string? searchQuery);

    Task<ChannelWithVideosDto> GetChannelAsync(long channelId);
    Task UploadAvatarAsync(long userId, Stream avatar, string fileName);
    Task UploadBannerAsync(long userId, Stream banner, string fileName);
}
