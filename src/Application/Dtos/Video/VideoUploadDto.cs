using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Video;

public class VideoUploadDto
{
    public string Title { get; set; }
    public string? Description { get; set; }

    public List<long>? TagIds { get; set; } // agar taglar biriktirilsa
}
