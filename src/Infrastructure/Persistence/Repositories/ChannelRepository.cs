using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ChannelRepository(AppDbContext appDbContext) : IChannelRepository
{
    public async Task<Channel> AddAsync(Channel channel)
    {
        await appDbContext.AddAsync(channel);
        await appDbContext.SaveChangesAsync();
        return channel;
    }

    public IQueryable<Channel> GetAllAsync()
    {
        var channels = appDbContext.Channels;
        return channels;
    }

    public Task<Channel?> GetByIdAsync(long id)
    {
        var channel = appDbContext.Channels
            .Include(c => c.Videos)
            .ThenInclude(c => c.ViewHistories)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
        return channel;
    }

    public Task<Channel?> GetByOwnerIdAsync(long userId)
    {
        var channel = appDbContext.Channels
            .Include(ch => ch.Videos)
            .ThenInclude(v => v.ViewHistories)
            .Include(c => c.Subscribers)
            .Include(c => c.Playlists)
            .FirstOrDefaultAsync(c => c.OwnerId == userId);
        return channel;
    }

    public Task<bool> IsChannelNameTakenAsync(string name)
    {
        var exists = appDbContext.Channels
            .AsNoTracking()
            .AnyAsync(c => c.Name == name);
        return exists;
    }

    public Task UpdateAsync(Channel channel)
    {
        appDbContext.Update(channel);
        return appDbContext.SaveChangesAsync();
    }
}
