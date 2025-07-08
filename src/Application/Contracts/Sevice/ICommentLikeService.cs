namespace Application.Contracts.Sevice;

public interface ICommentLikeService
{
    Task<bool> ToggleLikeAsync(long commentId, long userId);
    Task<int> GetLikeCountAsync(long commentId);
}