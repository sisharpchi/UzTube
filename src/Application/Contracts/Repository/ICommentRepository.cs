using Domain.Entities;

namespace Application.Contracts.Repository;

public interface ICommentRepository
{
    Task<Comment?> GetByIdAsync(long id);
    Task<IEnumerable<Comment>> GetByVideoIdAsync(long videoId);
    Task<IEnumerable<Comment>> GetRepliesAsync(long parentId);
    Task<long> AddAsync(Comment comment);
    Task DeleteAsync(long id);
    Task<int> CountAllCommentsByChannelId(long userId, long channelId);
}
