using Application.Dtos.LikeDislike;

namespace Application.Contracts.Sevice;

public interface ILikeDislikeService
{
    // 1. Add like or dislike to a video
    Task<long> ToggleAsync(long userId, LikeDislikeCreateDto dto);

    // 3. Get like/dislike count for a video
    Task<VideoLikeDislikeStatDto> GetStatAsync(long videoId);

    // 4. Check if a user has liked or disliked a video
    //Task<LikeDislikeStatusDto?> GetUserReactionAsync(long videoId, long userId);
}
