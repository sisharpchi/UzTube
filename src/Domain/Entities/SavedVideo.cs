namespace Domain.Entities;

public class SavedVideo
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long VideoId { get; set; }
    public Video Video { get; set; } = null!;
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
}
