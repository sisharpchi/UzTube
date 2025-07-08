using Application.Dtos.Video;

namespace Application.Contracts.Sevice;

public interface IVideoReportService
{
    Task<bool> ReportAsync(VideoReportCreateDto dto);
    Task<List<VideoReportDto>> GetReportsByVideoAsync(long videoId);
    Task<bool> ModerateAsync(long reportId, bool isApproved);
}