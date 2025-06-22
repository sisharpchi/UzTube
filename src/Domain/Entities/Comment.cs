namespace Domain.Entities;

public class Comment
{
    public long Id { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public long VideoId { get; set; }
    public Video Video { get; set; } = null!;

    public long UserId { get; set; }
    public User User { get; set; } = null!;

    public long? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }

    public ICollection<Comment> Replies { get; set; }
    public ICollection<CommentLike> Likes { get; set; }
}
