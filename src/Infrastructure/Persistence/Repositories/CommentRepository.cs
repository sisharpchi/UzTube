using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CommentRepository(AppDbContext appDbContext) : ICommentRepository
{
    public async Task<long> AddAsync(Comment comment)
    {
        await appDbContext.AddAsync(comment);
        await appDbContext.SaveChangesAsync();
        return comment.Id;
    }

    public async Task DeleteAsync(long id)
    {
        var existingComment = appDbContext.Comments.Find(id);
        appDbContext.Remove(existingComment);
        await appDbContext.SaveChangesAsync();
    }

    public Task<Comment?> GetByIdAsync(long id)
    {
        var comment = appDbContext.Comments
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
        return comment;
    }

    public async Task<IEnumerable<Comment>> GetByVideoIdAsync(long videoId)
    {
        var comments = await appDbContext.Comments
            .AsNoTracking()
            .Where(c => c.VideoId == videoId)
            .ToListAsync();
        return comments;
    }

    public async Task<IEnumerable<Comment>> GetRepliesAsync(long parentId)
    {
        var replies = await appDbContext.Comments
            .AsNoTracking()
            .Where(c => c.ParentCommentId == parentId)
            .ToListAsync();
        return replies;
    }
}
