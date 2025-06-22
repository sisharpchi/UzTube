namespace Domain.Entities;

public class VideoTag
{
    public long VideoId { get; set; }
    public Video Video { get; set; } = null!;

    public long TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
