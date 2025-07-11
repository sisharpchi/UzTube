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

        // 🔹 Toggle Subscribe / Unsubscribe
        group.MapPost("/toggle", async (
            HttpContext context,
            SubscriptionCreateDto dto,
            ISubscriptionService subscriptionService) =>
        {
            long userId = context.User.GetUserId();

            var result = await subscriptionService.ToggleSubscriptionAsync(userId, dto);
            return Results.Ok(new { status = result });
        });

        // 🔹 Get User Subscribed Channels
        group.MapGet("/my", async (
            HttpContext context,
            ISubscriptionService subscriptionService) =>
        {
            long userId = context.User.GetUserId();

            var channels = await subscriptionService.GetUserSubscriptionsAsync(userId);
            return Results.Ok(channels);
        });

        // 🔹 Get Subscriber Count for a Channel
        group.MapGet("/channel/{channelId:long}/count", async (
            long channelId,
            ISubscriptionService subscriptionService) =>
        {
            var count = await subscriptionService.GetSubscriberCountAsync(channelId);
            return Results.Ok(new { count });
        });

        // 🔹 Check if user is subscribed to channel
        group.MapGet("/channel/{channelId:long}/is-subscribed", async (
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