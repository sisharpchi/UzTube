namespace Domain.Entities;

public class LikeDislike
{
    public long Id { get; set; }
    public bool IsLike { get; set; }

    public long VideoId { get; set; }
    public Video Video { get; set; } = null!;

    public long UserId { get; set; }
    public User User { get; set; } = null!;
}
