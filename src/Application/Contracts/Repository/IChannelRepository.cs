using Domain.Entities;

namespace Application.Contracts.Repository;

public interface IChannelRepository
{
    Task<Channel?> GetByIdAsync(long id);
    Task<Channel?> GetByOwnerIdAsync(long userId);
    Task<IEnumerable<Channel>> GetAllAsync();
    Task<long> AddAsync(Channel channel);
    Task UpdateAsync(Channel channel);
    Task<bool> IsChannelNameTakenAsync(string name);
}
