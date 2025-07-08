using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ViewHistoryRepository(AppDbContext appDbContext) : IViewHistoryRepository
{
    public async Task<long> AddAsync(ViewHistory history)
    {
        await appDbContext.AddAsync(history);
        await appDbContext.SaveChangesAsync();
        return history.Id;
    }

    public async Task<ViewHistory> GetAsync(long userId, long videoId)
    {
        return await appDbContext.ViewHistories
            .FirstOrDefaultAsync(v => v.UserId == userId && v.VideoId == videoId);
    }

    public async Task<IEnumerable<ViewHistory>> GetByUserIdAsync(long userId)
    {
        return await appDbContext.ViewHistories
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.WatchedAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(ViewHistory history)
    {
        appDbContext.ViewHistories.Update(history);
        await appDbContext.SaveChangesAsync();
    }
}
