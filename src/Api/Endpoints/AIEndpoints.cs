using Application.Contracts.Sevice;
using Application.Dtos.AI;

namespace Api.Endpoints;

public static class AIEndpoints
{
    public static void MapAIEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/ai")
            .WithTags("AI");

        group.MapPost("/", async (
            ChatRequestDto dto,
            IAIService service) =>
        {
            var result = await service.GetChatResponseAsync(dto);
            return Results.Ok(result);
        });
    }
}
