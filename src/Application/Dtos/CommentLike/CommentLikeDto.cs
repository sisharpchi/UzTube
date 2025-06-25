namespace Application.Dtos.CommentLike;

public class CommentLikeDto
{
    public long Id { get; set; }
    public long CommentId { get; set; }
    public long UserId { get; set; }
    public string Username { get; set; } // optional
}