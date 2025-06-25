namespace Application.Dtos.LikeDislike;

public class VideoLikeDislikeStatDto
{
    public long VideoId { get; set; }
    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
}