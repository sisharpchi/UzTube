using Application.Dtos.Video;

namespace Application.Contracts.Sevicel;

public interface ISavedVideoService
{
    Task<bool> SaveAsync(long userId, long videoId);
    Task<bool> RemoveAsync(long userId, long videoId);
    Task<List<VideoListItemDto>> GetSavedVideosAsync(long userId);
}