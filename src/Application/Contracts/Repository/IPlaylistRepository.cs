using Domain.Entities;

namespace Application.Contracts.Repository;

public interface IPlaylistRepository
{
    Task<Playlist?> GetByIdAsync(long id);
    Task<IEnumerable<Playlist>> GetAllAsync();
    Task<long> AddAsync(Playlist playlist);
}
