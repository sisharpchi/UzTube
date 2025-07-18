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

        // POST: Upload video
        userGroup.MapPost("", async (
            HttpContext httpContext,
            [FromForm] VideoUploadDto videoUploadDto,
            IFormFile file, // asosiy video fayl
            IVideoService uploadService) =>
        {
            var userIdClaim = httpContext.User.FindFirst("UserId");
            if (userIdClaim == null)
                return Results.Unauthorized();

            var userId = long.Parse(userIdClaim.Value);

            if (file == null || file.Length == 0)
                return Results.BadRequest("Fayl topilmadi.");

            await using var stream = file.OpenReadStream();
            var fileName = file.FileName;

            // Thumbnail faylni videoUploadDto ichidan olamiz
            var result = await uploadService.UploadVideoOrImageAsync(userId, videoUploadDto, stream, fileName);

            return Results.Ok(result);
        })
        .DisableAntiforgery();

        // DELETE: Delete video by nodeId
        userGroup.MapDelete("", async (
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

        // GET: Get all videos
        userGroup.MapGet("/all", async (IVideoService videoService) =>
        {
            var result = await videoService.GetAllVideosAsync();
            return Results.Ok(result);
        }).AllowAnonymous();

        // GET: Get video by Id
        userGroup.MapGet("/{id:long}", async (long id, IVideoService videoService) =>
        {
            var result = await videoService.GetByIdAsync(id);
            return Results.Ok(result);
        });

        // GET: Get videos by userId (foydalanuvchining videolari)
        userGroup.MapGet("/user", async (HttpContext context, IVideoService videoService) =>
        {
            var userIdClaim = context.User.FindFirst("UserId");
            if (userIdClaim == null)
                return Results.Unauthorized();

            var userId = long.Parse(userIdClaim.Value);
            var result = await videoService.GetByUserAsync(userId);
            return Results.Ok(result);
        });

        // GET: Get videos by channelId
        userGroup.MapGet("/channel/{channelId:long}", async (long channelId, IVideoService videoService) =>
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
