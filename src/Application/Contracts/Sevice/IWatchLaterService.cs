using Application.Dtos.Video;

namespace Application.Contracts.Sevice;

public interface IWatchLaterService
{
    Task<bool> AddAsync(long userId, long videoId);
    Task<bool> RemoveAsync(long userId, long videoId);
    Task<List<VideoListItemDto>> GetListAsync(long userId);
}