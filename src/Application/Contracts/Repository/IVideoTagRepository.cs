using Domain.Entities;

namespace Application.Contracts.Repository;

public interface IVideoTagRepository
{
    Task<long> AddAsync(VideoTag tag);
    Task RemoveAsync(long id);
    Task<IEnumerable<Tag>> GetTagsByVideoIdAsync(long videoId);
}