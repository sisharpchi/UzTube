namespace Application.Dtos.Video;

public class VideoReportCreateDto
{
    public long VideoId { get; set; }         // Qaysi videoga report
    public long UserId { get; set; }          // Kim report qilayapti
    public string Reason { get; set; }        // Nima sababdan
}