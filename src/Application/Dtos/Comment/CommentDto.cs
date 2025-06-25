namespace Application.Dtos.Comment;

public class CommentDto
{
    public long Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }

    public long VideoId { get; set; }
    public long UserId { get; set; }
    public string Username { get; set; } // optional: foydalanuvchi nomi

    public long? ParentCommentId { get; set; }
    public int LikeCount { get; set; }
    public List<CommentDto> Replies { get; set; }
}