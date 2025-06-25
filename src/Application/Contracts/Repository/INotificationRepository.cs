using Domain.Entities;

namespace Application.Contracts.Repository;


public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetByUserIdAsync(long userId);
    Task<long> AddAsync(Notification notification);
    Task RemoveAsync(long id);
    Task MarkAllAsReadAsync(long userId);
}
