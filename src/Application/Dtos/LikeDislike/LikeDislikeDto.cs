namespace Application.Dtos.LikeDislike;

public class LikeDislikeDto
{
    public long Id { get; set; }
    public bool IsLike { get; set; }

    public long VideoId { get; set; }
    public long UserId { get; set; }
    public string Username { get; set; } // optional
}