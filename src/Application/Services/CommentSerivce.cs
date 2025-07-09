using Application.Contracts.Repository;
using Application.Contracts.Sevice;
using Application.Dtos.Comment;
using Core.Errors;
using Domain.Entities;

namespace Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IVideoRepository videoRepository;
    private readonly ICommentLikeRepository commentLikeRepository;

    public CommentService(
        IVideoRepository videoRepository,
        IUserRepository userRepository,
        ICommentRepository commentRepository,
        ICommentLikeRepository commentLikeRepository)
    {
        this.videoRepository = videoRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.commentLikeRepository = commentLikeRepository;
    }

    public async Task<long> AddAsync(long userId, CommentCreateDto dto)
    {
        var commentCreateDto = new CommentCreateDto
        {
            Text = dto.Text,
            VideoId = dto.VideoId,
            ParentCommentId = dto.ParentCommentId,
        };

        var comment = ConvertToComment(commentCreateDto);
        comment.UserId = userId;

        return await commentRepository.AddAsync(comment);
    }

    private Comment ConvertToComment(CommentCreateDto commentCreateDto)
    {
        return new Comment()
        {
            Text = commentCreateDto.Text,
            VideoId = commentCreateDto.VideoId,
            ParentCommentId = commentCreateDto.ParentCommentId
        };
    }

    public async Task<int> CountByVideoIdAsync(long videoId)
    {
        var count = await videoRepository.GetByIdAsync(videoId);
        return count.Comments?.Count() ?? 0;
    }

    public async Task DeleteAsync(long commentId, long userId)
    {
        var comment = await commentRepository.GetByIdAsync(commentId);
        if (comment.UserId == userId)
        {
            await commentRepository.DeleteAsync(commentId);
        }
    }

    public async Task<List<CommentDto>> GetByVideoIdAsync(long userId, long videoId)
    {
        var comments = await commentRepository.GetByVideoIdAsync(videoId);
        return comments.Select(c => ConvertToDto(c, userId)).ToList();
    }

    private CommentDto ConvertToDto(Comment c, long userId)
    {
        return new CommentDto()
        {
            Id = c.Id,
            Text = c.Text,
            CreatedAt = c.CreatedAt,
            VideoId = c.VideoId,
            UserId = c.UserId,
            ParentCommentId = c.ParentCommentId,
            Username = userRepository.GetByIdAsync(userId).Result.FullName,
            LikeCount = c.Likes?.Count ?? 0,
            IsLikedByCurrentUser = c.Likes?.Any(l => l.UserId == userId) ?? false,
            Replies = c.Replies?
                .OrderBy(r => r.CreatedAt)
                .Select(r => ConvertToDto(r, userId)) // recursive
                .ToList() ?? new List<CommentDto>()
        };
    }

    public async Task<List<CommentDto>> GetRepliesAsync(long parentCommentId)
    {
        var replies = await commentRepository.GetRepliesAsync(parentCommentId);
        return replies.Select(r => ConvertToDto(r, r.UserId)).ToList();
    }

    public async Task<bool> ToggleLikeAsync(long commentId, long userId)
    {
        var existingLike = await commentLikeRepository.GetByUserAndCommentIdAsync(userId, commentId);

        if (existingLike == null)
        {
            var newLike = new CommentLike
            {
                CommentId = commentId,
                UserId = userId,
            };

            await commentLikeRepository.AddAsync(newLike);
            return true;
        }
        else
        {
            await commentLikeRepository.RemoveAsync(existingLike.Id);
            return false; 
        } 
    }
}