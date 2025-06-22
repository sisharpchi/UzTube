namespace Domain.Entities;

public class CommentLike
{
    public long Id { get; set; }
    public long CommentId { get; set; }
    public Comment Comment { get; set; } = null!;

    public long UserId { get; set; }
    public User User { get; set; } = null!;
}
