namespace Domain.Entities;

public class ViewHistory
{
    public long Id { get; set; }
    public DateTime WatchedAt { get; set; }
    public int SecondsWatched { get; set; }

    public long VideoId { get; set; }
    public Video Video { get; set; } = null!;

    public long UserId { get; set; }
}
