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

        // 🔹 Toggle Like/Dislike
        group.MapPost("/toggle", async (
            HttpContext context,
            LikeDislikeCreateDto dto,
            ILikeDislikeService service) =>
        {
            long userId = context.User.GetUserId();

            var id = await service.ToggleAsync(userId, dto);
            return Results.Ok(new { id });
        });

        // 🔹 Get Like/Dislike Stats by VideoId
        group.MapGet("/video/{videoId:long}/stats", async (
            long videoId,
            HttpContext context,
            ILikeDislikeService service) =>
        {
            long userId = context.User.GetUserId(); // agar kerak bo‘lsa, ishlatiladi

            var stats = await service.GetStatAsync(videoId);
            return Results.Ok(stats);
        });
    }
}
