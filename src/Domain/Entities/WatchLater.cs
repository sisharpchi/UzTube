namespace Domain.Entities;

public class WatchLater
{
    public long Id { get; set; }

    public long UserId { get; set; }
    public User User { get; set; } = null!;

    public long VideoId { get; set; }
    public Video Video { get; set; } = null!;
}
