using Domain.Entities;

namespace Application.Contracts.Repository;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(long id);
    Task<User?> GetByEmailAsync(string email);
    IQueryable<User> GetAll();
    Task<long> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(long id);
    Task<bool> ExistsAsync(string email);
}