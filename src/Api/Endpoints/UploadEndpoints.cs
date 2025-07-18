using Application.Contracts.Sevice;
using Application.Dtos.Video;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class UploadEndpoints
{
    public static void MapVideoEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/video")
            .WithTags("Video")
            .RequireAuthorization();

        userGroup.MapPost("/create", async (
            HttpContext httpContext,
            [FromForm] VideoUploadDto videoUploadDto,
            IFormFile thumbnail,
            IFormFile file,
            IVideoService uploadService) =>
        {
            var userIdClaim = httpContext.User.FindFirst("UserId");
            if (userIdClaim == null)
                return Results.Unauthorized();

            var userId = long.Parse(userIdClaim.Value);

            if (file == null || file.Length == 0)
                return Results.BadRequest("Fayl topilmadi.");

            if (thumbnail == null || file.Length == 0)
                return Results.BadRequest("Thumbnail");

            await using var stream = file.OpenReadStream();
            var fileName = $"{userId}_{file.Name}";

            await using var stremThumbnail = thumbnail.OpenReadStream();
            var thumbnailName = $"{userId}_{thumbnail.Name}";

            var result = await uploadService.UploadVideoOrImageAsync(userId, videoUploadDto, stream, fileName, stremThumbnail, thumbnailName);

            return Results.Ok(result);
        })
        .DisableAntiforgery();

        userGroup.MapDelete("/delete/{nodeId}", async (
            string nodeId,
            HttpContext httpContext,
            IVideoService uploadService) =>
        {
            var userIdClaim = httpContext.User.FindFirst("UserId");
            if (userIdClaim == null)
                return Results.Unauthorized();

            var userId = long.Parse(userIdClaim.Value);

            if (string.IsNullOrWhiteSpace(nodeId))
                return Results.BadRequest("NodeId bo‘sh bo‘lishi mumkin emas.");

            await uploadService.DeleteFileAsync(userId, nodeId);
            return Results.Ok("Fayl o‘chirildi.");
        });

        userGroup.MapGet("/all", async (IVideoService videoService) =>
        {
            var result = await videoService.GetAllVideosAsync();
            return Results.Ok(result);
        })
        .AllowAnonymous();

        userGroup.MapGet("/{id:long}", async (long id, IVideoService videoService) =>
        {
            var result = await videoService.GetByIdAsync(id);
            return Results.Ok(result);
        })
        .AllowAnonymous();

        userGroup.MapGet("/my", async (HttpContext context, IVideoService videoService) =>
        {
            var userIdClaim = context.User.FindFirst("UserId");
            if (userIdClaim == null)
                return Results.Unauthorized();

            var userId = long.Parse(userIdClaim.Value);
            var result = await videoService.GetByUserAsync(userId);
            return Results.Ok(result);
        });

        userGroup.MapGet("{channelId:long}", async (long channelId, IVideoService videoService) =>
        {
            var result = await videoService.GetByChannelAsync(channelId);
            return Results.Ok(result);
        });




        // GET: Get videos by playlistId
        userGroup.MapGet("/playlist/{playlistId:long}", async (long playlistId, IVideoService videoService) =>
        {
            var result = await videoService.GetByPlaylistAsync(playlistId);
            return Results.Ok(result);
        });

        // GET: Get videos by tagId
        userGroup.MapGet("/tag/{tagId:long}", async (long tagId, IVideoService videoService) =>
        {
            var result = await videoService.GetByTagAsync(tagId);
            return Results.Ok(result);
        });

        // GET: Get trending videos
        userGroup.MapGet("/trending", async ([FromQuery] int count, IVideoService videoService) =>
        {
            var result = await videoService.GetTrendingAsync(count);
            return Results.Ok(result);
        });

        // PUT: Update video
        userGroup.MapPut("/{videoId:long}", async (
            long videoId,
            VideoUpdateDto dto,
            IVideoService videoService) =>
        {
            var result = await videoService.UpdateAsync(videoId, dto);
            return Results.Ok(result);
        });

        // POST: Add view
        userGroup.MapPost("/{videoId:long}/view", async (
            long videoId,
            HttpContext context,
            IVideoService videoService) =>
        {
            var userIdClaim = context.User.FindFirst("UserId");
            if (userIdClaim == null)
                return Results.Unauthorized();

            var userId = long.Parse(userIdClaim.Value);
            await videoService.AddViewAsync(videoId, userId);
            return Results.Ok();
        });
    }
}
