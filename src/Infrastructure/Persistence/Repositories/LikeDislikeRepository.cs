using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class LikeDislikeRepository(AppDbContext appDbContext) : ILikeDislikeRepository
{
    public async Task<long> AddAsync(LikeDislike like)
    {
        await appDbContext.AddAsync(like);
        await appDbContext.SaveChangesAsync();
        return like.Id;
    }

    public async Task<(int likes, int dislikes)> CountByVideoIdAsync(long videoId)
    {
        var result = await appDbContext.LikeDislikes
            .Where(cl => cl.VideoId == videoId)
            .GroupBy(cl => cl.IsLike)
            .Select(g => new { IsLiked = g.Key, Count = g.Count() })
            .ToListAsync();

        int likes = result.FirstOrDefault(r => r.IsLiked == true)?.Count ?? 0;
        int dislikes = result.FirstOrDefault(r => r.IsLiked == false)?.Count ?? 0;

        return (likes, dislikes);
    }

    public Task<LikeDislike?> GetByUserAndVideoIdAsync(long userId, long videoId)
    {
        return appDbContext.LikeDislikes
            .FirstOrDefaultAsync(ld => ld.UserId == userId && ld.VideoId == videoId);
    }

    public async Task RemoveAsync(long id)
    {
        var likeDislike = await appDbContext.LikeDislikes.FindAsync(id);
        appDbContext.LikeDislikes.Remove(likeDislike);
        await appDbContext.SaveChangesAsync();
    }
}
