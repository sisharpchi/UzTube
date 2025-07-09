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
            .RequireAuthorization(); // JWT authentication talab qilinadi

        group.MapPost("/", async (
            HttpContext context,
            IChannelService service,
            [FromBody] ChannelCreateDto dto) =>
        {
            long userId = context.User.GetUserId();
            var id = await service.CreateAsync(userId, dto);
            return Results.Ok(id);
        });

        group.MapGet("/", async (
            HttpContext context,
            IChannelService service) =>
        {
            long userId = context.User.GetUserId();
            var result = await service.GetByUserIdAsync(userId);
            return Results.Ok(result);
        });

        group.MapGet("/videos", async (
            HttpContext context,
            IChannelService service) =>
        {
            long userId = context.User.GetUserId();
            var result = await service.GetWithVideosAsync(userId);
            return Results.Ok(result);
        });

        group.MapPut("/", async (
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
            return Results.Ok(count);
        });

        group.MapGet("/search", async (
            HttpContext context,
            [FromServices] IChannelService service,
            [FromQuery] string? query) =>
        {
            long userId = context.User.GetUserId();
            var result = service.SearchAsync(query);
            return Results.Ok(result);
        });
    }
}