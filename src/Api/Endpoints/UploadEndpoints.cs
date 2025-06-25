using Application.Contracts.Sevice;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class UploadEndpoints
{
    public static IEndpointRouteBuilder MapUploadEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/uploadVideoOrImage", async (IFormFile file, IUploadService uploadService) =>
        {
            if (file == null || file.Length == 0)
                return Results.BadRequest("Fayl topilmadi.");

            await using var stream = file.OpenReadStream();
            var fileName = file.FileName;

            var result = await uploadService.UploadVideoOrImageAsync(stream, fileName);

            return Results.Ok(result); // { fileUrl, nodeId }
        })
        .DisableAntiforgery();

        app.MapDelete("/api/delete", async (string nodeId, IUploadService uploadService) =>
        {
            if (string.IsNullOrWhiteSpace(nodeId))
                return Results.BadRequest("NodeId bo‘sh bo‘lishi mumkin emas.");

            await uploadService.DeleteFileAsync(nodeId);
            return Results.Ok("Fayl o‘chirildi.");
        });

        return app;
    }
}