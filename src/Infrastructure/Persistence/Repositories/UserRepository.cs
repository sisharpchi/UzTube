using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    public async Task<long> AddAsync(User user)
    {
        await appDbContext.AddAsync(user);
        await appDbContext.SaveChangesAsync();
        return user.Id;
    }

    public async Task DeleteAsync(long id)
    {
        var existingUser = appDbContext.Users.Find(id);
        appDbContext.Remove(existingUser);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string email)
    {
        var exists = await appDbContext.Users.AsNoTracking().AnyAsync(u => u.Email == email);
        return exists;
    }

    public IQueryable<User> GetAll()
    {
        var users = appDbContext.Users.AsNoTracking();
        return users;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await appDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        var user = await appDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
        
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        appDbContext.Update(user);
        await appDbContext.SaveChangesAsync();
    }
}
