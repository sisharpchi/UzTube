using Domain.Entities;

namespace Application.Contracts.Repository;

public interface ICommentLikeRepository
{
    Task<CommentLike?> GetByUserAndCommentIdAsync(long userId, long commentId);
    Task<long> AddAsync(CommentLike like);
    Task RemoveAsync(long id);
    Task<int> CountByCommentIdAsync(long commentId);
}