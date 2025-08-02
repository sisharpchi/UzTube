using Application.Contracts.Repository;
using Application.Contracts.Sevice;
using Application.Dtos.Channel;
using Application.Dtos.Upload;
using Application.Dtos.Video;
using CG.Web.MegaApiClient;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Services;

public class VideoService : IVideoService
{
    //private readonly MegaApiClient megaApiClient;
    private readonly Cloudinary cloudinary;
    private readonly IVideoRepository videoRepository;
    private readonly IViewHistoryRepository viewHistoryRepository;
    private readonly IChannelRepository channelRepository;
    private readonly IUserRepository userRepository;

    public VideoService(MegaApiClient megaApiClient, IVideoRepository videoRepository, IViewHistoryRepository viewHistoryRepository, IChannelRepository channelRepository, Cloudinary cloudinary, IUserRepository userRepository)
    {
        //this.megaApiClient = megaApiClient;
        this.videoRepository = videoRepository;
        this.viewHistoryRepository = viewHistoryRepository;
        this.channelRepository = channelRepository;
        this.cloudinary = cloudinary;
        this.userRepository = userRepository;
    }

    public async Task<bool> AddViewAsync(long videoId, long userId)
    {
        var existingView = await viewHistoryRepository.GetAsync(userId, videoId);
        if (existingView is null)
        {
            var newView = new ViewHistory
            {
                VideoId = videoId,
                UserId = userId,
                WatchedAt = DateTime.UtcNow,
                SecondsWatched = 0, // Boshlang'ich qiymat
            };

            await viewHistoryRepository.AddAsync(newView);
        }
        else
        {
            // Faqat viewedAt vaqtini yangilash (optional)
            existingView.WatchedAt = DateTime.UtcNow;
            await viewHistoryRepository.UpdateAsync(existingView);
        }
        return true;
    }

    public async Task DeleteFileAsync(long userId, string publicId)
    {
        // 1. Videoni topamiz
        var video = await videoRepository.GetByNodeAndOwnerId(userId, publicId);
        if (video is null)
            throw new InvalidOperationException("Video topilmadi yoki sizga tegishli emas.");

        // 2. Cloudinarydan video va thumbnailni o‘chiramiz
        var deleteVideoParams = new DeletionParams(video.CloudPublicId)
        {
            ResourceType = ResourceType.Video
        };

        var deleteVideoResult = await cloudinary.DestroyAsync(deleteVideoParams);

        if (deleteVideoResult.Result != "ok" && deleteVideoResult.Result != "not_found")
            throw new Exception($"Videoni o‘chirishda xatolik: {deleteVideoResult.Result}");

        if (!string.IsNullOrEmpty(video.ThumbnailUrl))
        {
            // Thumbnail publicId ni URL dan ajratib olish
            var thumbnailPublicId = GetPublicIdFromUrl(video.ThumbnailUrl);

            var deleteImageParams = new DeletionParams(thumbnailPublicId);
            var deleteImageResult = await cloudinary.DestroyAsync(deleteImageParams);

            if (deleteImageResult.Result != "ok" && deleteImageResult.Result != "not_found")
                throw new Exception($"Thumbnailni o‘chirishda xatolik: {deleteImageResult.Result}");
        }

        // 3. Videoni bazadan o‘chiramiz yoki flag belgilaymiz
        await videoRepository.DeleteAsync(video.Id); // yoki IsDeleted = true
    }

    private string GetPublicIdFromUrl(string url)
    {
        var uri = new Uri(url);
        var segments = uri.Segments;
        var fileName = segments.Last(); // example: `thumbnail_abc123.jpg`
        var folderPath = string.Join("", segments.Skip(segments.Length - 2)).Trim('/'); // example: `thumbnails/thumbnail_abc123.jpg`
        var publicIdWithExtension = folderPath;

        return Path.ChangeExtension(publicIdWithExtension, null); // Remove extension
    }

    public async Task<List<VideoDto>> GetAllVideosAsync()
    {
        var videos = await videoRepository.GetAllAsync();
        return videos.Select(v => ConvertToVideoDto(v)).ToList();
    }

    public async Task<List<VideoListItemDto>> GetByChannelAsync(long channelId)
    {
        var channel = await channelRepository.GetByIdAsync(channelId);
        if (channel == null || channel.Videos == null)
            return new List<VideoListItemDto>();

        var videos = channel.Videos;

        var result = videos.Select(video => new VideoListItemDto
        {
            Id = video.Id,
            Title = video.Title,
            ThumbnailUrl = video.ThumbnailUrl,
            Duration = video.Duration,
            ChannelName = channel.Name,
            UploadedAt = video.UploadedAt,
            ViewCount = video.ViewHistories?.Count ?? 0
        }).ToList();

        return result;
    }

    public async Task<VideoDto> GetByIdAsync(long videoId)
    {
        return ConvertToVideoDto(await videoRepository.GetByIdAsync(videoId));
    }

    public Task<List<VideoListItemDto>> GetByPlaylistAsync(long playlistId)
    {
        throw new NotImplementedException();
    }

    public Task<List<VideoListItemDto>> GetByTagAsync(long tagId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<VideoListItemDto>> GetByUserAsync(long userId)
    {
        var channel = await channelRepository.GetByOwnerIdAsync(userId);
        if (channel == null || channel.Videos == null)
            return new List<VideoListItemDto>();

        var result = channel.Videos.Select(v => new VideoListItemDto
        {
            Id = v.Id,
            Title = v.Title,
            ThumbnailUrl = v.ThumbnailUrl,
            Duration = v.Duration,
            ChannelName = channel.Name,
            UploadedAt = v.UploadedAt,
            ViewCount = v.ViewHistories?.Count ?? 0,
        }).ToList();

        return result;
    }

    public Task<List<VideoListItemDto>> GetTrendingAsync(int count = 20)
    {
        throw new NotImplementedException();
    }

    public Task<VideoDto> UpdateAsync(long videoId, VideoUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Dtos.Upload.UploadResult> UploadVideoOrImageAsync(
            long userId,
            VideoUploadDto videoUpload,
            Stream fileStream,
            string fileName,
            Stream thumbnailStream,
            string thumbnailName)
    {
        var channel = await channelRepository.GetByOwnerIdAsync(userId);

        // 🎥 Video faylni yuklash
        var videoUploadParams = new VideoUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = "videos",
        };

        var videoUploadResult = await cloudinary.UploadAsync(videoUploadParams);

        if (videoUploadResult.Error != null)
            throw new Exception($"Video upload failed: {videoUploadResult.Error.Message}");

        // 🕒 Duration ni olish
        double durationInSeconds = videoUploadResult?.Duration ?? 0; // Cloudinary video duration returns in seconds

        // 🖼 Thumbnail faylni yuklash
        var imageUploadParams = new ImageUploadParams
        {
            File = new FileDescription(thumbnailName, thumbnailStream),
            Folder = "thumbnails"
        };

        var imageUploadResult = await cloudinary.UploadAsync(imageUploadParams);

        if (imageUploadResult.Error != null)
            throw new Exception($"Thumbnail upload failed: {imageUploadResult.Error.Message}");

        // 🎞 VideoEntity yaratish
        var videoEntity = new Domain.Entities.Video
        {
            Title = videoUpload.Title,
            Description = videoUpload.Description,
            VideoUrl = videoUploadResult.SecureUrl.ToString(),
            CloudPublicId = videoUploadResult.PublicId,
            Duration = TimeSpan.FromSeconds(durationInSeconds),
            ChannelId = channel.Id,
            ThumbnailUrl = imageUploadResult.SecureUrl.ToString(),
            UploadedAt = DateTime.UtcNow,
            Channel = channel,
        };

        await videoRepository.AddAsync(videoEntity);

        return new Dtos.Upload.UploadResult
        {
            FileUrl = videoUploadResult.SecureUrl.ToString(),
            NodeId = videoUploadResult.PublicId
        };
    }

    private VideoDto ConvertToVideoDto(Domain.Entities.Video video)
    {
        return new VideoDto
        {
            Id = video.Id,
            Title = video.Title,
            Description = video.Description,
            VideoUrl = video.VideoUrl,
            UploadedAt = video.UploadedAt,
            ChannelId = video.ChannelId,
            ThumbnailUrl = video.ThumbnailUrl,
            Duration = video.Duration,
            Channel = ConvertChannelToDto(video.Channel),
            DislikeCount = video.Likes.Count(l => l.IsLike != true),
            LikeCount = video.Likes.Count(l => l.IsLike == true),
            ViewCount = video.ViewHistories.Count,
            PlaylistId = video.PlaylistId,
            PlaylistName = video.Playlist?.Name
        };
    }


    private ChannelDto ConvertChannelToDto(Domain.Entities.Channel channel)
    {
        return new ChannelDto
        {
            Id = channel.Id,
            Name = channel.Name,
            Description = channel.Description,
            OwnerUsername = userRepository.GetByIdAsync(channel.OwnerId).Result!.FullName,
            OwnerId = channel.OwnerId,
            VideoCount = channel.Videos?.Count ?? 0,
            SubscriberCount = channel.Subscribers?.Count ?? 0,
            PlaylistCount = channel.Playlists?.Count ?? 0
        };
    }

    private Domain.Entities.Video ConvertVideoUploadDtoToEntity(VideoUploadDto dto)
    {
        return new Domain.Entities.Video
        {
            Title = dto.Title,
            Description = dto.Description,
        };
    }
}
