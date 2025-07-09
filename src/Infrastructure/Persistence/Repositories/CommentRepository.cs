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
            .Where(c => c.VideoId == videoId)
            .Include(c => c.Likes)
            .Include(c => c.Replies)
            .ThenInclude(r => r.Likes)
            .AsNoTracking()
            .ToListAsync();

        foreach (var comment in comments)
        {
            comment.Replies = comments.Where(c => c.ParentCommentId == comment.Id).ToList();
        }

        return comments.Where(c => c.ParentCommentId == null).ToList();
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
