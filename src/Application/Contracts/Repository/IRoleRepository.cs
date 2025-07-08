using Domain.Entities;
using System.Data;

namespace Application.Contracts.Repository;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(long id);
    Task<IEnumerable<Role>> GetAllAsync();
    Task<long> AddAsync(Role role);
    Task UpdateAsync(Role role);
    Task<long> GetRoleIdAsync(string role);
}