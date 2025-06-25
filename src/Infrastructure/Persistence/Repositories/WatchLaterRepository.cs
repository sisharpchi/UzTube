using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class WatchLaterRepository(AppDbContext appDbContext) : IWatchLaterRepository
{
    public async Task<long> AddAsync(WatchLater watchLater)
    {
        await appDbContext.WatchLaters.AddAsync(watchLater);
        await appDbContext.SaveChangesAsync();
        return watchLater.Id;
    }

    public async Task<bool> ExistsAsync(long userId, long videoId)
    {
        var exists = await appDbContext.WatchLaters
            .AsNoTracking()
            .AnyAsync(wl => wl.UserId == userId && wl.VideoId == videoId);
        return exists;
    }

    public async Task<IEnumerable<WatchLater>> GetByUserIdAsync(long userId)
    {
        var watchLaters = await appDbContext.WatchLaters
            .AsNoTracking()
            .Where(wl => wl.UserId == userId)
            .ToListAsync();

        return watchLaters;
    }

    public async Task RemoveAsync(long id)
    {
        var watchLater = appDbContext.WatchLaters.Find(id);
        appDbContext.WatchLaters.Remove(watchLater);
        await appDbContext.SaveChangesAsync();
    }
}
