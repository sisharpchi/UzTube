using Domain.Entities;

namespace Application.Contracts.Repository;

public interface ILikeDislikeRepository
{
    Task<LikeDislike?> GetByUserAndVideoIdAsync(long userId, long videoId);
    Task<long> AddAsync(LikeDislike like);
    Task RemoveAsync(long id);
    Task<(int likes, int dislikes)> CountByVideoIdAsync(long videoId);
    Task UpdateAsync(LikeDislike entity);
    Task<int> CountAllLikesByChannelId(long userId, long channelId);
    Task<int> CountAllDislikesByChannelId(long userId, long channelId);
}