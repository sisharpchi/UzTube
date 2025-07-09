using Application.Contracts.Repository;
using Application.Contracts.Sevice;
using Application.Dtos.Channel;
using Application.Dtos.Video;
using Domain.Entities;

namespace Application.Services;

public class ChanelService(IChannelRepository channelRepository) : IChannelService
{
    public async Task<long> CreateAsync(long userId, ChannelCreateDto dto)
    {
        var existsChanel = await channelRepository.GetByOwnerIdAsync(userId);
        if (existsChanel != null)
        {
            throw new InvalidOperationException("Sizda allaqachon kanal mavjud.");
        }

        var channelDto = new ChannelCreateDto()
        {
            Description = dto.Description,
            Name = dto.Name,
        };
        var channel = ConvertDtoToChannel(channelDto);
        channel.OwnerId = userId;

        return await channelRepository.AddAsync(channel);
    }

    public async Task<ChannelDto> GetByUserIdAsync(long userId)
    {
        var channel = await channelRepository.GetByOwnerIdAsync(userId);
        if (channel is null)
        {
            throw new InvalidOperationException("Kanal topilmadi.");
        }

        return ConvertChannelToDto(channel);
    }

    public async Task<int> GetSubscriberCountAsync(long userId)
    {
        var channel = await channelRepository.GetByOwnerIdAsync(userId);
        if (channel is null)
        {
            throw new InvalidOperationException("Kanal topilmadi.");
        }
        return channel.Subscribers?.Count ?? 0;
    }

    public async Task<ChannelWithVideosDto> GetWithVideosAsync(long userId)
    {
        var channel = await channelRepository.GetByOwnerIdAsync(userId);
        if (channel is null)
        {
            throw new InvalidOperationException("Kanal topilmadi.");
        }

        var channelWithVideoDto = new ChannelWithVideosDto()
        {
            Id = channel.Id,
            Description = channel.Description,
            Name = channel.Name,
            Videos = channel.Videos?.Select(video => new VideoDto
            {
                Id = video.Id,
                Title = video.Title,
                Description = video.Description,
                VideoUrl = video.VideoUrl,
                ThumbnailUrl = video.ThumbnailUrl,
                Duration = video.Duration,
                ChannelId = channel.Id,
                LikeCount = video.Likes.Count(l => l.IsLike == true),
                DislikeCount = video.Likes.Count(d => d.IsLike != true),
                UploadedAt = video.UploadedAt,
                ChannelName = channel.Name,
                PlaylistName = video.Playlist?.Name,
                PlaylistId = video.Playlist?.Id,
                ViewCount = video.ViewHistories.Count(),
            }).ToList()!
        };
        return channelWithVideoDto;
    }

    public List<ChannelListItemDto> SearchAsync(string? searchQuery)
    {
        var query = channelRepository.GetAllAsync();
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(x =>
                x.Name.Contains(searchQuery) ||
                x.Description.Contains(searchQuery));
        }

        var channels = query.ToList().Select(q => ConvertToChanelListItemDto(q));
        return channels.ToList();
    }

    private ChannelListItemDto ConvertToChanelListItemDto(Channel channel)
    {
        return new ChannelListItemDto()
        {
            Id = channel.Id,
            Name = channel.Name,
            SubscriberCount = channel.Subscribers?.Count ?? 0
        };
    }

    public async Task UpdateAsync(long userId, ChannelUpdateDto dto)
    {
        var channel = await channelRepository.GetByOwnerIdAsync(userId);
        if (channel == null)
        {
            throw new InvalidOperationException("Kanal topilmadi.");
        }

        var channelUpdateDto = new ChannelUpdateDto()
        {
            Description = dto.Description,
            Name = dto.Name
        };

        var channelUpdate = ConvertDtoToChannel(channelUpdateDto);
        channelUpdate.Id = channel.Id;

        await channelRepository.UpdateAsync(channelUpdate);
    }

    private Channel ConvertDtoToChannel(ChannelCreateDto channelCreateDto)
    {
        return new Channel()
        {
            Description = channelCreateDto.Description,
            Name = channelCreateDto.Name,
        };
    }

    private Channel ConvertDtoToChannel(ChannelUpdateDto channelUpdateDto)
    {
        return new Channel()
        {
            Description = channelUpdateDto.Description,
            Name = channelUpdateDto.Name
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
