using Application.Contracts.Repository;
using Application.Contracts.Sevice;
using Application.Dtos.Channel;
using Application.Dtos.Video;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using System.IO;

namespace Application.Services;

public class ChanelService(IChannelRepository channelRepository, IUserRepository userRepository, Cloudinary cloudinary) : IChannelService
{
    public async Task<ChannelWithVideosDto> CreateAsync(long userId, ChannelCreateDto dto)
    {
        var existsChanel = await channelRepository.GetByOwnerIdAsync(userId);
        if (existsChanel != null)
        {
            throw new InvalidOperationException("Sizda allaqachon kanal mavjud.");
        }

        var channel = ConvertDtoToChannel(dto);
        channel.OwnerId = userId;
        
        var channelResponse = await channelRepository.AddAsync(channel);
        return ConvertToDto(channelResponse);
    }

    public async Task<ChannelWithVideosDto> GetByUserIdAsync(long userId)
    {
        var channel = await channelRepository.GetByOwnerIdAsync(userId);
        if (channel is null)
        {
            throw new InvalidOperationException("Kanal topilmadi.");
        }

        return ConvertToDto(channel);
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

        return ConvertToDto(channel);
    }
    private ChannelWithVideosDto ConvertToDto(Channel channel)
    {
        return new ChannelWithVideosDto()
        {
            Id = channel.Id,
            Description = channel.Description,
            Name = channel.Name,
            AvatarUrl = channel.AvatarUrl,
            AvatarCloudPublicId = channel.AvatarCloudPublicId,
            BannerUrl = channel.BannerUrl,
            BannerCloudPublicId = channel.BannerCloudPublicId,
            Videos = channel.Videos?.Select(video => new VideoDto
            {
                Id = video.Id,
                Title = video.Title,
                Description = video.Description,
                VideoUrl = video.VideoUrl,
                ThumbnailUrl = video.ThumbnailUrl,
                Duration = video.Duration,
                UploadedAt = video.UploadedAt,
                ChannelId = channel.Id,
                PlaylistId = video.Playlist?.Id,
                PlaylistName = video.Playlist?.Name,

                LikeCount = video.Likes?.Count(l => l.IsLike == true) ?? 0,
                DislikeCount = video.Likes?.Count(d => d.IsLike != true) ?? 0,
                ViewCount = video.ViewHistories?.Count() ?? 0,

            }).ToList() ?? new List<VideoDto>()
        };
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
            ChannelDto = ConvertChannelToDto(channel)
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

    public async Task<ChannelWithVideosDto> GetChannelAsync(long channelId)
    {
        var channel = await channelRepository.GetByIdAsync(channelId);
        return ConvertToDto(channel!);
    }

    public async Task UploadAvatarAsync(long userId, Stream avatar, string fileName)
    {
        var existingChannel = await channelRepository.GetByOwnerIdAsync(userId);
        if (existingChannel == null)
            throw new InvalidOperationException("Kanal topilmadi.");
        
        if (!string.IsNullOrEmpty(existingChannel.AvatarUrl) || !string.IsNullOrEmpty(existingChannel.AvatarCloudPublicId))
        {
            // 2. Cloudinarydan video va thumbnailni o‘chiramiz
            var deleteImageParams = new DeletionParams(existingChannel.AvatarCloudPublicId)
            {
                ResourceType = ResourceType.Image
            };

            var deleteImageResult = await cloudinary.DestroyAsync(deleteImageParams);

            if (deleteImageResult.Result != "ok" && deleteImageResult.Result != "not_found")
                throw new Exception($"Imageni o‘chirishda xatolik: {deleteImageResult.Result}");

            existingChannel.AvatarUrl = null;
            existingChannel.AvatarCloudPublicId = null;
        }

        var imageUploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, avatar),
            Folder = "ChannelAvatars",
        };

        var imageUploadResult = await cloudinary.UploadAsync(imageUploadParams);

        if (imageUploadResult.Error != null)
            throw new Exception($"Image upload failed: {imageUploadResult.Error.Message}");


        existingChannel.AvatarUrl = imageUploadResult.SecureUrl.ToString();
        existingChannel.AvatarCloudPublicId = imageUploadResult.PublicId;
        await channelRepository.UpdateAsync(existingChannel);
    }

    public async Task UploadBannerAsync(long userId, Stream banner, string fileName)
    {
        var existingChannel = await channelRepository.GetByOwnerIdAsync(userId);
        if (existingChannel == null)
            throw new InvalidOperationException("Kanal topilmadi.");

        if (!string.IsNullOrEmpty(existingChannel.BannerUrl) || !string.IsNullOrEmpty(existingChannel.BannerCloudPublicId))
        {
            // Delete old banner from Cloudinary
            var deleteBannerParams = new DeletionParams(existingChannel.BannerCloudPublicId)
            {
                ResourceType = ResourceType.Image
            };

            var deleteBannerResult = await cloudinary.DestroyAsync(deleteBannerParams);

            if (deleteBannerResult.Result != "ok" && deleteBannerResult.Result != "not_found")
                throw new Exception($"Bannerni o‘chirishda xatolik: {deleteBannerResult.Result}");

            existingChannel.BannerUrl = null;
            existingChannel.BannerCloudPublicId = null;
        }

        var bannerUploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, banner),
            Folder = "ChannelBanners",
        };

        var bannerUploadResult = await cloudinary.UploadAsync(bannerUploadParams);

        if (bannerUploadResult.Error != null)
            throw new Exception($"Banner upload failed: {bannerUploadResult.Error.Message}");

        existingChannel.BannerUrl = bannerUploadResult.SecureUrl.ToString();
        existingChannel.BannerCloudPublicId = bannerUploadResult.PublicId;

        await channelRepository.UpdateAsync(existingChannel);
    }

}
