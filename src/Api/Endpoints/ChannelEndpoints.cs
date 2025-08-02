using Api.Extensions;
using Application.Contracts.Sevice;
using Application.Dtos.Channel;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class ChannelEndpoints
{
    public static void MapChannelEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/channel")
            .WithTags("Channel")
            .RequireAuthorization();

        group.MapPost("/create", async (
            HttpContext context,
            IChannelService service,
            [FromBody] ChannelCreateDto dto) =>
        {
            long userId = context.User.GetUserId();
            var channelWithVideosDto = await service.CreateAsync(userId, dto);
            return Results.Ok(new { data = channelWithVideosDto });
        });

        group.MapGet("/my", async (
            HttpContext context,
            IChannelService service) =>
        {
            long userId = context.User.GetUserId();
            var result = await service.GetByUserIdAsync(userId);
            return Results.Ok(new { data = result });
        });

        group.MapGet("/my-videos", async (
            HttpContext context,
            IChannelService service) =>
        {
            long userId = context.User.GetUserId();
            var result = await service.GetWithVideosAsync(userId);
            return Results.Ok(new { result });
        });

        group.MapPut("/update", async (
            HttpContext context,
            IChannelService service,
            [FromBody] ChannelUpdateDto dto) =>
        {
            long userId = context.User.GetUserId();
            await service.UpdateAsync(userId, dto);
            return Results.NoContent();
        });

        group.MapGet("/subscribers-count", async (
            HttpContext context,
            IChannelService service) =>
        {
            long userId = context.User.GetUserId();
            var count = await service.GetSubscriberCountAsync(userId);
            return Results.Ok(new { data = count });
        });

        group.MapGet("/search/{query}", async (
            HttpContext context,
            [FromServices] IChannelService service,
            [FromQuery] string? query) =>
        {
            long userId = context.User.GetUserId();
            var result = service.SearchAsync(query);
            return Results.Ok(result);
        });

        group.MapGet("/{channelId:long}", async (
            long channelId,
            IChannelService service) =>
        {
            var result = await service.GetChannelAsync(channelId);
            return Results.Ok(new { data = result });
        })
        .RequireAuthorization();

        group.MapPost("/upload-avatar", async (
            HttpContext context,
            IChannelService service,
            IFormFile file) =>
        {
            if (file == null || file.Length == 0)
                return Results.BadRequest("File is required.");

            long userId = context.User.GetUserId();
            using var stream = file.OpenReadStream();
            await service.UploadAvatarAsync(userId, stream, file.FileName);
            return Results.Ok(new { data = true });
        })
        .DisableAntiforgery();

        group.MapPost("/upload-banner", async (
            HttpContext context,
            IChannelService service,
            IFormFile file) =>
        {
            if (file == null || file.Length == 0)
                return Results.BadRequest("File is required.");

            long userId = context.User.GetUserId();
            using var stream = file.OpenReadStream();
            await service.UploadBannerAsync(userId, stream, file.FileName);
            return Results.Ok(new { data = true });
        })
        .DisableAntiforgery();
    }
}