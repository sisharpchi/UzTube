using Application.Dtos.Video;

namespace Application.Contracts.Sevice;

public interface IVideoService : IUploadService
{
    // 1. Upload new video
    //Task<VideoDto> UploadAsync(VideoUploadDto dto);

    // 2. Get video by ID
    Task<VideoDto> GetByIdAsync(long videoId);

    // 3. Get video with full detail (comments, tags, likes)
    //Task<VideoDetailDto> GetDetailedByIdAsync(long videoId);

    // 4. Get trending videos (e.g., by view count or recent uploads)
    Task<List<VideoListItemDto>> GetTrendingAsync(int count = 20);

    // 5. Add a view to video (view history)
    Task<bool> AddViewAsync(long videoId, long userId);

    // 6. Update video info
    Task<VideoDto> UpdateAsync(long videoId, VideoUpdateDto dto);

    // 7. Delete video
    //Task<bool> DeleteAsync(long videoId);

    // 8. Get videos by tag
    Task<List<VideoListItemDto>> GetByTagAsync(long tagId);

    // 9. Get videos by playlist
    Task<List<VideoListItemDto>> GetByPlaylistAsync(long playlistId);

    // 10. Get videos by channel
    Task<List<VideoListItemDto>> GetByChannelAsync(long channelId);

    // 11. Get videos by user (uploaded videos)
    Task<List<VideoListItemDto>> GetByUserAsync(long userId);

    // 12. Get video analytics (views, likes, comments count)
    //Task<VideoAnalyticsDto> GetAnalyticsAsync(long videoId);
    Task<List<VideoDto>> GetAllVideosAsync();
}
