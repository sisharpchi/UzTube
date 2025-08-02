using Api.Extensions;
using Application.Contracts.Sevice;
using Application.Dtos.LikeDislike;

namespace Api.Endpoints;

public static class LikeDislikeEndpoints
{
    public static void MapLikeDislikeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/likedislike")
            .WithTags("LikeDislike")
            .RequireAuthorization();

        group.MapPost("/toggle", async (
            HttpContext context,
            LikeDislikeCreateDto dto,
            ILikeDislikeService service) =>
        {
            long userId = context.User.GetUserId();

            var id = await service.ToggleAsync(userId, dto);
            return Results.Ok(new { id });
        });

        group.MapGet("/stats/{videoId:long}", async (
            long videoId,
            HttpContext context,
            ILikeDislikeService service) =>
        {
            long userId = context.User.GetUserId();

            var stats = await service.GetStatAsync(videoId);
           
            return Results.Ok(new { success = true, data = stats });
        });

        group.MapGet("reactions/{videoId:long}", async (
            long videoId,
            HttpContext context,
            ILikeDislikeService service) =>
        {
            long userId = context.User.GetUserId();
            var response = await service.GetUserReactionAsync(videoId, userId);
            return Results.Ok(new { data = response });
        });
    }
}
