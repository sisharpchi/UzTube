﻿using Application.Dtos.Comment;

namespace Application.Contracts.Sevice;

public interface ICommentService
{
    // 1. Add a new comment to a video or reply
    Task<long> AddAsync(long userId, CommentCreateDto dto);

    // 3. Get comments by video ID (faqat root commentlar)
    Task<List<CommentDto>> GetByVideoIdAsync(long userId, long videoId);

    // 4. Get replies of a comment (child commentlar)
    Task<List<CommentDto>> GetRepliesAsync(long parentCommentId);

    // 5. Like/unlike a comment
    Task<bool> ToggleLikeAsync(long commentId, long userId);

    // 6. Delete comment (user yoki admin o‘chirishi mumkin)
    Task DeleteAsync(long commentId, long userId);

    // 7. Count comments for a video
    Task<int> CountByVideoIdAsync(long videoId);
}