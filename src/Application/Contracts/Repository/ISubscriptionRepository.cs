using Domain.Entities;

namespace Application.Contracts.Repository;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetByUserAndChannelIdAsync(long userId, long channelId);
    Task<IEnumerable<Subscription>> GetByUserIdAsync(long userId);
    Task<long> AddAsync(Subscription subscription);
    Task DeleteAsync(long id);
    Task<int> CountSubscribersAsync(long channelId);
}
