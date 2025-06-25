using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PlaylistRepository(AppDbContext appDbContext) : IPlaylistRepository
{
    public async Task<long> AddAsync(Playlist playlist)
    {
        await appDbContext.Playlists.AddAsync(playlist);
        await appDbContext.SaveChangesAsync();
        return playlist.Id;
    }

    public async Task<IEnumerable<Playlist>> GetAllAsync()
    {
        return await appDbContext.Playlists
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<Playlist?> GetByIdAsync(long id)
    {
        return appDbContext.Playlists
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
