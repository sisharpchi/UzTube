using Application.Contracts.Repository;
using Core.Errors;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RoleRepository(AuthDbContext authDbContext) : IRoleRepository
{
    public async Task<long> AddAsync(Role role)
    {
        await authDbContext.Roles.AddAsync(role);
        await authDbContext.SaveChangesAsync();
        return role.Id;
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        var roles = await authDbContext.Roles.ToListAsync();
        return roles;
    }

    public Task<Role?> GetByIdAsync(long id)
    {
        var role = authDbContext.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

        return role;
    }

    public async Task<long> GetRoleIdAsync(string role)
    {
        var foundRole = await authDbContext.Roles.FirstOrDefaultAsync(_ => _.Name == role);
        if (foundRole is null)
        {
            throw new EntityNotFoundException(role + " - not found");
        }
        return foundRole.Id;
    }

    public async Task UpdateAsync(Role role)
    {
        authDbContext.Roles.Update(role);
        await authDbContext.SaveChangesAsync();
    }
}
