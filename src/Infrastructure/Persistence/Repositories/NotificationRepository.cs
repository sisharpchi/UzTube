using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class NotificationRepository(AppDbContext appDbContext) : INotificationRepository
{
    public async Task<long> AddAsync(Notification notification)
    {
        await appDbContext.Notifications.AddAsync(notification);
        await appDbContext.SaveChangesAsync();
        return notification.Id;
    }

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(long userId)
    {
        var notifications = await appDbContext.Notifications
            .Where(n => n.UserId == userId)
            .ToListAsync();

        return notifications;
    }

    public async Task MarkAllAsReadAsync(long userId)
    {
        var notifications = await appDbContext.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();
        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }
        appDbContext.Notifications.UpdateRange(notifications);
        await appDbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(long id)
    {
        var notification = await appDbContext.Notifications.FindAsync(id);
        appDbContext.Notifications.Remove(notification);
        await appDbContext.SaveChangesAsync();
    }
}
