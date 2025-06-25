namespace Application.Dtos.LikeDislike;

public class LikeDislikeCreateDto
{
    public bool IsLike { get; set; }
    public long VideoId { get; set; }
    public long UserId { get; set; }
}