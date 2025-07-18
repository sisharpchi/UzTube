using Api.Extensions;
using Application.Contracts.Sevice;
using Application.Dtos.Comment;

namespace Api.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/comment")
            .WithTags("Comment")
            .RequireAuthorization();

        group.MapPost("/create", async (
            HttpContext context,
            CommentCreateDto dto,
            ICommentService commentService) =>
        {
            long userId = context.User.GetUserId();

            var commentId = await commentService.AddAsync(userId, dto);
            return Results.Ok(commentId);
        });

        group.MapGet("/all/{videoId:long}", async (
            long videoId,
            HttpContext context,
            ICommentService commentService) =>
        {
            long userId = context.User.GetUserId();

            var comments = await commentService.GetByVideoIdAsync(userId, videoId);
            return Results.Ok(comments);
        });

        group.MapDelete("/delete/{commentId:long}", async (
            long commentId,
            HttpContext context,
            ICommentService commentService) =>
        {
            long userId = context.User.GetUserId();

            await commentService.DeleteAsync(commentId, userId);
            return Results.Ok("Deleted");
        });

        // 🔹 Toggle Like
        group.MapPost("/toggle/{commentId:long}", async (
            long commentId,
            HttpContext context,
            ICommentService commentService) =>
        {
            long userId = context.User.GetUserId();

            var liked = await commentService.ToggleLikeAsync(commentId, userId);
            return Results.Ok(new { liked });
        });

        // 🔹 Get Replies
        group.MapGet("/replies/{commentId:long}", async (
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