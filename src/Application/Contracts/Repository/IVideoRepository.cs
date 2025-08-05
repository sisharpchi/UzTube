using Domain.Entities;

namespace Application.Contracts.Repository;

public interface IVideoRepository
{
    Task<Video?> GetByIdAsync(long userId, long id);
    Task<Video?> GetByIdAsync(long id);
    Task<Video> GetByNodeAndOwnerId(long userId, string nodeId);
    Task<IEnumerable<Video>> GetAllAsync();
    Task<IEnumerable<Video>> GetByChannelIdAsync(long channelId);
    Task<IEnumerable<Video>> SearchAsync(string query);
    Task<IEnumerable<Video>> FilterAsync(string? tag, long? channelId, string? duration, string? sort);
    Task<long> AddAsync(Video video);
    Task UpdateAsync(Video video);
    Task DeleteAsync(long id);
    Task IncrementViewCountAsync(long videoId);
}
