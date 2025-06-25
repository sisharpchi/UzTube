namespace Application.Dtos.SavedVideo;

public class SavedVideoDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long VideoId { get; set; }

    public string VideoTitle { get; set; }
    public string VideoThumbnailUrl { get; set; }
    public DateTime SavedAt { get; set; } // agar qo‘shilsa, entitiyga ham qo‘shish kerak
}