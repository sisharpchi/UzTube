using Domain.Entities;

namespace Application.Contracts.Repository;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetAllAsync();
    Task<Tag?> GetByIdAsync(long id);
    Task<Tag?> GetByNameAsync(string name);
    Task<long> AddAsync(Tag tag);
}
