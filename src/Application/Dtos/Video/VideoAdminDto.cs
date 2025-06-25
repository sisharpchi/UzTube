namespace Application.Dtos.Video;

public class VideoAdminDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string ChannelName { get; set; }
    public DateTime UploadedAt { get; set; }

    public int ReportCount { get; set; }
    public int LikeCount { get; set; }
    public int ViewCount { get; set; }

    public bool IsFlagged { get; set; } // agar reportlar belgilansa
}