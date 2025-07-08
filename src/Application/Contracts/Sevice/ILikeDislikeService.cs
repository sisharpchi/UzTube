using Application.Dtos.LikeDislike;

namespace Application.Contracts.Sevice;

public interface ILikeDislikeService
{
    // 1. Add like or dislike to a video
    Task<bool> AddAsync(LikeDislikeCreateDto dto);

    // 2. Remove like/dislike from a video
    Task<bool> RemoveAsync(long videoId, long userId);

    // 3. Get like/dislike count for a video
    Task<VideoLikeDislikeStatDto> GetStatAsync(long videoId);

    // 4. Check if a user has liked or disliked a video
    //Task<LikeDislikeStatusDto?> GetUserReactionAsync(long videoId, long userId);
}
