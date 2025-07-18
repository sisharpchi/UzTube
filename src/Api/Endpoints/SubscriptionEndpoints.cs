using Api.Extensions;
using Application.Contracts.Sevice;
using Application.Dtos.Subscription;

namespace Api.Endpoints;

public static class SubscriptionEndpoints
{
    public static void MapSubscriptionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/subscriptions")
            .WithTags("Subscriptions")
            .RequireAuthorization();

        group.MapPost("/toggle", async (
            HttpContext context,
            SubscriptionCreateDto dto,
            ISubscriptionService subscriptionService) =>
        {
            long userId = context.User.GetUserId();

            var result = await subscriptionService.ToggleSubscriptionAsync(userId, dto);
            return Results.Ok(new { status = result });
        });

        group.MapGet("/my-subscriptions", async (
            HttpContext context,
            ISubscriptionService subscriptionService) =>
        {
            long userId = context.User.GetUserId();

            var channels = await subscriptionService.GetUserSubscriptionsAsync(userId);
            return Results.Ok(channels);
        });

        group.MapGet("/subscribers/{channelId:long}/", async (
            long channelId,
            ISubscriptionService subscriptionService) =>
        {
            var count = await subscriptionService.GetSubscriberCountAsync(channelId);
            return Results.Ok(new { count });
        });

        group.MapGet("/is-subscribed/{channelId:long}", async (
            long channelId,
            HttpContext context,
            ISubscriptionService subscriptionService) =>
        {
            long userId = context.User.GetUserId();

            var isSubscribed = await subscriptionService.IsSubscribedAsync(userId, channelId);
            return Results.Ok(new { isSubscribed });
        });
    }
}