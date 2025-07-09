using Api.Extensions;
using Application.Contracts.Sevice;
using Application.Dtos.Comment;

namespace Api.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/commnet")
            .WithTags("Comment")
            .RequireAuthorization();

        // 🔹 Add Comment
        group.MapPost("/", async (
            HttpContext context,
            CommentCreateDto dto,
            ICommentService commentService) =>
        {
            long userId = context.User.GetUserId();

            var commentId = await commentService.AddAsync(userId, dto);
            return Results.Ok(commentId);
        });

        // 🔹 Get Comments by VideoId
        group.MapGet("/video/{videoId:long}", async (
            long videoId,
            HttpContext context,
            ICommentService commentService) =>
        {
            long userId = context.User.GetUserId();

            var comments = await commentService.GetByVideoIdAsync(userId, videoId);
            return Results.Ok(comments);
        });

        // 🔹 Delete Comment
        group.MapDelete("/{commentId:long}", async (
            long commentId,
            HttpContext context,
            ICommentService commentService) =>
        {
            long userId = context.User.GetUserId();

            await commentService.DeleteAsync(commentId, userId);
            return Results.Ok("Deleted");
        });

        // 🔹 Toggle Like
        group.MapPost("/{commentId:long}/like", async (
            long commentId,
            HttpContext context,
            ICommentService commentService) =>
        {
            long userId = context.User.GetUserId();

            var liked = await commentService.ToggleLikeAsync(commentId, userId);
            return Results.Ok(new { liked });
        });

        // 🔹 Get Replies
        group.MapGet("/{commentId:long}/replies", async (
            HttpContext context,
            long commentId,
            ICommentService commentService) =>
        {
            long userId = context.User.GetUserId();

            var replies = await commentService.GetRepliesAsync(commentId);
            return Results.Ok(replies);
        });
    }
}