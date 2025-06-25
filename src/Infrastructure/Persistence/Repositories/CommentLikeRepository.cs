using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CommentLikeRepository(AppDbContext appDbContext) : ICommentLikeRepository
{
    public async Task<long> AddAsync(CommentLike like)
    {
        await appDbContext.AddAsync(like);
        await appDbContext.SaveChangesAsync();
        return like.Id;
    }

    public async Task<int> CountByCommentIdAsync(long commentId)
    {
        return await appDbContext.CommentLikes.CountAsync(cl => cl.CommentId == commentId);
    }

    public Task<CommentLike?> GetByUserAndCommentIdAsync(long userId, long commentId)
    {
        return appDbContext.CommentLikes
            .FirstOrDefaultAsync(cl => cl.UserId == userId && cl.CommentId == commentId);
    }

    public async Task RemoveAsync(long id)
    {
        var like = await appDbContext.CommentLikes
            .FirstOrDefaultAsync(c => c.Id == id);
        appDbContext.CommentLikes.Remove(like!);
        await appDbContext.SaveChangesAsync();
    }
}
