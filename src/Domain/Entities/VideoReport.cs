namespace Domain.Entities;

public class VideoReport
{
    public long Id { get; set; }
    public string Reason { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public long VideoId { get; set; }
    public Video Video { get; set; } = null!;

    public long ReporterId { get; set; }
}
