namespace Application.Dtos.Comment;

public class CommentListItemDto
{
    public long Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Username { get; set; }
}