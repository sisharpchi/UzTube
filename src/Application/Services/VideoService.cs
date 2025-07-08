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


    public async Task DeleteFileAsync(string nodeId)
    {
        var nodes = megaApiClient.GetNodes();
        var node = nodes.FirstOrDefault(x => x.Id == nodeId);

        if (node is null)
            throw new InvalidOperationException("Node topilmadi yoki sizga tegishli emas.");

        megaApiClient.Delete(node, moveToTrash: false);
    }

    public Task<List<VideoListItemDto>> GetByChannelAsync(long channelId)
    {
        throw new NotImplementedException();
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

    public Task<List<VideoListItemDto>> GetByUserAsync(long userId)
    {
        throw new NotImplementedException();
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
        // Root papkani olish
        var chanel = await channelRepository.GetByOwnerIdAsync(userId);
        var nodes = await megaApiClient.GetNodesAsync();
        var root = nodes.Single(n => n.Type == NodeType.Root);
        // Faylni yuklash
        var uploadedNode = await megaApiClient.UploadAsync(fileStream, fileName, root);
        var fileUrl = megaApiClient.GetDownloadLink(uploadedNode).ToString();

        var mediaInfo = await FFmpeg.GetMediaInfo(fileUrl);

        var videoUploadDto = new VideoUploadDto
        {
            Title = videoUpload.Title,
            Description =  videoUpload.Description,
        };

        var videoEntity = ConvertVideoUploadDtoToEntity(videoUploadDto);

        videoEntity.VideoUrl = fileUrl;
        videoEntity.CloudPublicId = uploadedNode.Id;
        videoEntity.Duration = mediaInfo.Duration;
        videoEntity.ChannelId = chanel.Id;
        videoEntity.UploadedAt = DateTime.UtcNow;

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
