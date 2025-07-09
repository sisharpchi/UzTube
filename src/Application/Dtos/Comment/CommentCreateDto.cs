namespace Application.Dtos.Comment;

public class CommentCreateDto
{
    public string Text { get; set; }
    public long VideoId { get; set; }
    public long? ParentCommentId { get; set; }
}