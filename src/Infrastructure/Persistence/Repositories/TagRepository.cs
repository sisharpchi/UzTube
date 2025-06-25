using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TagRepository(AppDbContext appDbContext) : ITagRepository
{
    public async Task<long> AddAsync(Tag tag)
    {
        await appDbContext.AddAsync(tag);
        await appDbContext.SaveChangesAsync();
        return tag.Id;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await appDbContext.Tags
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Tag?> GetByIdAsync(long id)
    {
        return await appDbContext.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await appDbContext.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Name == name);
    }
}
