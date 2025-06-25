using Domain.Entities;

namespace Application.Contracts.Repository;

public interface IWatchLaterRepository
{
    Task<IEnumerable<WatchLater>> GetByUserIdAsync(long userId);
    Task<long> AddAsync(WatchLater watchLater);
    Task RemoveAsync(long id);
    Task<bool> ExistsAsync(long userId, long videoId);
}