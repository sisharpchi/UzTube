using Domain.Entities;

namespace Application.Contracts.Repository;

public interface IViewHistoryRepository
{
    Task<IEnumerable<ViewHistory>> GetByUserIdAsync(long userId);
    Task<long> AddAsync(ViewHistory history);
}
