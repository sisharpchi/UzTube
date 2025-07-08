using Application.Contracts.Sevice;
using Application.Dtos.Video;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class UploadEndpoints
{
    public static IEndpointRouteBuilder MapUploadEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/uploadVideoOrImage", async (
            HttpContext httpContext, // 🔥 bu yerda qo‘shiladi
            VideoUploadDto videoUploadDto,
            IFormFile file,
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

            var result = await uploadService.UploadVideoOrImageAsync(userId, videoUploadDto, stream, fileName);

            return Results.Ok(result); // { fileUrl, nodeId }
        })
        .RequireAuthorization() // JWT ishlashi uchun kerak
        .DisableAntiforgery();

        app.MapDelete("/api/delete", async (string nodeId, IVideoService uploadService) =>
        {
            if (string.IsNullOrWhiteSpace(nodeId))
                return Results.BadRequest("NodeId bo‘sh bo‘lishi mumkin emas.");

            await uploadService.DeleteFileAsync(nodeId);
            return Results.Ok("Fayl o‘chirildi.");
        });

        return app;
    }
}