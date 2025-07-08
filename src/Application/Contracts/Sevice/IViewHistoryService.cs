using Application.Dtos.Video;

namespace Application.Contracts.Sevice;

public interface IViewHistoryService
{
    Task<bool> AddAsync(long userId, long videoId);
    Task<List<VideoListItemDto>> GetHistoryAsync(long userId);
    Task<bool> ClearAsync(long userId);
}