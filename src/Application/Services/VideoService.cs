using Application.Contracts.Repository;
using Application.Contracts.Sevice;
using Application.Dtos.Upload;
using Application.Dtos.Video;
using CG.Web.MegaApiClient;
using Domain.Entities;
using Xabe.FFmpeg;

namespace Application.Services;

public class VideoService : IVideoService
{
    private readonly MegaApiClient megaApiClient;
    private readonly IVideoRepository videoRepository;
    private readonly IViewHistoryRepository viewHistoryRepository;
    private readonly IChannelRepository channelRepository;

    public VideoService(MegaApiClient megaApiClient, IVideoRepository videoRepository, IViewHistoryRepository viewHistoryRepository, IChannelRepository channelRepository)
    {
        this.megaApiClient = megaApiClient;
        this.videoRepository = videoRepository;
        this.viewHistoryRepository = viewHistoryRepository;
        this.channelRepository = channelRepository;
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

    public async Task DeleteFileAsync(long userId, string nodeId)
    {
        var video = await videoRepository.GetByNodeAndOwnerId(userId, nodeId);
        if (video is null)
            throw new InvalidOperationException("Node topilmadi yoki sizga tegishli emas.");

        var nodes = megaApiClient.GetNodes();
        var node = nodes.FirstOrDefault(x => x.Id == video.CloudPublicId);

        if (node is null)
            throw new InvalidOperationException("Node topilmadi yoki sizga tegishli emas.");

        megaApiClient.Delete(node, moveToTrash: true);
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

    public async Task<UploadResult> UploadVideoOrImageAsync(long userId, VideoUploadDto videoUpload, Stream fileStream, string fileName)
    {
        var chanel = await channelRepository.GetByOwnerIdAsync(userId);
        var nodes = await megaApiClient.GetNodesAsync();
        var root = nodes.Single(n => n.Type == NodeType.Root);

        // Video faylni yuklash
        var uploadedNode = await megaApiClient.UploadAsync(fileStream, fileName, root);
        var fileUrl = megaApiClient.GetDownloadLink(uploadedNode).ToString();

        // Thumbnail faylni yuklash (ixtiyoriy)
        string? thumbnailUrl = null;
        if (videoUpload.Thumbnail != null)
        {
            await using var thumbStream = videoUpload.Thumbnail.OpenReadStream();
            var uploadedThumbnail = await megaApiClient.UploadAsync(thumbStream, videoUpload.Thumbnail.FileName, root);
            thumbnailUrl = megaApiClient.GetDownloadLink(uploadedThumbnail).ToString();
        }

        // VideoEntity yaratish
        var videoEntity = new Video
        {
            Title = videoUpload.Title,
            Description = videoUpload.Description,
            VideoUrl = fileUrl,
            CloudPublicId = uploadedNode.Id,
            Duration = TimeSpan.FromSeconds(0), // Keyin FFmpeg orqali set qilinadi
            ChannelId = chanel.Id,
            ThumbnailUrl = thumbnailUrl,
            UploadedAt = DateTime.UtcNow
        };

        await videoRepository.AddAsync(videoEntity);

        return new UploadResult
        {
            FileUrl = fileUrl,
            NodeId = uploadedNode.Id
        };
    }

    private VideoDto ConvertToVideoDto(Video video)
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
            ChannelName = video.Channel?.Name,
            DislikeCount = video.Likes.Count(l => l.IsLike != true),
            LikeCount = video.Likes.Count(l => l.IsLike == true),
            ViewCount = video.ViewHistories.Count,
            PlaylistId = video.PlaylistId,
            PlaylistName = video.Playlist?.Name
        };
    }

    private Video ConvertVideoUploadDtoToEntity(VideoUploadDto dto)
    {
        return new Video
        {
            Title = dto.Title,
            Description = dto.Description,
        };
    }
}
