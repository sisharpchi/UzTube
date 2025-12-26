using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(AuthDbContext authDbContext) : IUserRepository
{
    public async Task<long> AddAsync(User user)
    {
        await authDbContext.AddAsync(user);
        await authDbContext.SaveChangesAsync();
        return user.Id;
    }

    public async Task DeleteAsync(long id)
    {
        var existingUser = authDbContext.Users.Find(id);
        authDbContext.Remove(existingUser);
        await authDbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string email)
    {
        var exists = await authDbContext.Users.AsNoTracking().AnyAsync(u => u.Email == email);
        return exists;
    }

    public IQueryable<User> GetAll()
    {
        var users = authDbContext.Users.AsNoTracking();
        return users;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await authDbContext.Users
            .Include(u => u.Confirmer)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        var user = await authDbContext.Users
            .Include(u => u.Confirmer)
            .Include(u => u.Role)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        authDbContext.Attach(user);
        authDbContext.Entry(user).State = EntityState.Modified;
        //authDbContext.Entry(user).State = EntityState.Modified;
        authDbContext.Update(user);
        await authDbContext.SaveChangesAsync();
    }
}
