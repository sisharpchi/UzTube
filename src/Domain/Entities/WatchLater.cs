namespace Domain.Entities;

public class WatchLater
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long VideoId { get; set; }
    public Video Video { get; set; } = null!;
}
