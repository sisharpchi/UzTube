using Domain.Entities;

namespace Application.Contracts.Repository;

public interface IChannelRepository
{
    Task<Channel?> GetByIdAsync(long id);
    Task<Channel?> GetByOwnerIdAsync(long userId);
    Task<bool> ExistsChannelByOwnerIdAsync(long id);
    IQueryable<Channel> GetAllAsync();
    Task<Channel> AddAsync(Channel channel);
    Task UpdateAsync(Channel channel);
    Task<bool> IsChannelNameTakenAsync(string name);
}
