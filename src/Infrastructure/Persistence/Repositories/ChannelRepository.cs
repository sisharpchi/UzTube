using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ChannelRepository(AppDbContext appDbContext) : IChannelRepository
{
    public async Task<long> AddAsync(Channel channel)
    {
        await appDbContext.AddAsync(channel);
        await appDbContext.SaveChangesAsync();
        return channel.Id;
    }

    public async Task<IEnumerable<Channel>> GetAllAsync()
    {
        var channels = await appDbContext.Channels.AsNoTracking().ToListAsync();
        return channels;
    }

    public Task<Channel?> GetByIdAsync(long id)
    {
        var channel = appDbContext.Channels
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
        return channel;
    }

    public Task<Channel?> GetByOwnerIdAsync(long userId)
    {
        var channel = appDbContext.Channels
            .AsNoTracking()
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
