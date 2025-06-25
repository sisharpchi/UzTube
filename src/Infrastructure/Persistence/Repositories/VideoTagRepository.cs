using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;

namespace Infrastructure.Persistence.Repositories;

public class VideoTagRepository(AppDbContext appDbContext) : IVideoTagRepository
{
    public Task<long> AddAsync(VideoTag tag)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tag>> GetTagsByVideoIdAsync(long videoId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(long id)
    {
        throw new NotImplementedException();
    }
}
