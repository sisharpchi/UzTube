using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RoleRepository(AppDbContext appDbContext) : IRoleRepository
{
    public async Task<long> AddAsync(Role role)
    {
        await appDbContext.Roles.AddAsync(role);
        await appDbContext.SaveChangesAsync();
        return role.Id;
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        var roles = await appDbContext.Roles.ToListAsync();
        return roles;
    }

    public Task<Role?> GetByIdAsync(long id)
    {
        var role = appDbContext.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

        return role;
    }

    public async Task UpdateAsync(Role role)
    {
        appDbContext.Roles.Update(role);
        await appDbContext.SaveChangesAsync();
    }
}
