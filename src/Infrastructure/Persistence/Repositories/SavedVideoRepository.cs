using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SavedVideoRepository(AppDbContext appDbContext) : ISavedVideoRepository
{
    public async Task<long> AddAsync(SavedVideo saved)
    {
        await appDbContext.AddAsync(saved);
        await appDbContext.SaveChangesAsync();
        return saved.Id;
    }

    public Task<bool> ExistsAsync(long userId, long videoId)
    {
        var exist = appDbContext.SavedVideos
            .AsNoTracking()
            .AnyAsync(sv => sv.UserId == userId && sv.VideoId == videoId);
        return exist;
    }

    public async Task<IEnumerable<SavedVideo>> GetByUserIdAsync(long userId)
    {
        var savedVideos = await appDbContext.SavedVideos
            .AsNoTracking()
            .Where(sv => sv.UserId == userId)
            .ToListAsync();
        return savedVideos;
    }

    public async Task RemoveAsync(long id)
    {
        var savedVideo = await appDbContext.SavedVideos.FindAsync(id);
        appDbContext.SavedVideos.Remove(savedVideo);
        await appDbContext.SaveChangesAsync();
    }
}
