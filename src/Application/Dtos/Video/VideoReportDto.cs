namespace Application.Dtos.Video;

public class VideoReportDto
{
    public long Id { get; set; }
    public long VideoId { get; set; }
    public string VideoTitle { get; set; }

    public long UserId { get; set; }
    public string UserFullName { get; set; }

    public string Reason { get; set; }
    public DateTime ReportedAt { get; set; }

    public bool? IsReviewed { get; set; }     // null = hali ko‘rilmagan
    public bool? IsApproved { get; set; }     // true = tasdiqlangan (video muammo), false = rad etilgan
}
