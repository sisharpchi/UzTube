using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SubscriptionRepository(AppDbContext appDbContext) : ISubscriptionRepository
{
    public async Task<long> AddAsync(Subscription subscription)
    {
        await appDbContext.Subscriptions.AddAsync(subscription);
        await appDbContext.SaveChangesAsync();
        return subscription.Id;
    }

    public async Task<int> CountSubscribersAsync(long channelId)
    {
        var count = await appDbContext.Subscriptions.AsNoTracking()
            .CountAsync(s => s.ChannelId == channelId);
        return count;
    }

    public async Task DeleteAsync(long userId, long channelId)
    {
        var existingSubscription = await appDbContext.Subscriptions.FirstOrDefaultAsync(s => s.ChannelId == channelId && s.SubscriberId == userId);
        appDbContext.Subscriptions.Remove(existingSubscription);
        await appDbContext.SaveChangesAsync();
    }

    public Task<Subscription?> GetByUserAndChannelIdAsync(long userId, long channelId)
    {
        return appDbContext.Subscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SubscriberId == userId && s.ChannelId == channelId);
    }

    public async Task<IEnumerable<Subscription>> GetByUserIdAsync(long userId)
    {
        return await appDbContext.Subscriptions
            .Include(s => s.Channel)
            .ThenInclude(c => c.Subscribers)
            .Where(s => s.SubscriberId == userId)
            .ToListAsync();
    }
}
