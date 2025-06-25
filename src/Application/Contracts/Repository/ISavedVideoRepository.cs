using Domain.Entities;

namespace Application.Contracts.Repository;


public interface ISavedVideoRepository
{
    Task<IEnumerable<SavedVideo>> GetByUserIdAsync(long userId);
    Task<long> AddAsync(SavedVideo saved);
    Task RemoveAsync(long id);
    Task<bool> ExistsAsync(long userId, long videoId);
}
